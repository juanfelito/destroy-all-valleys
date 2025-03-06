using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GameObject draggedItem;

    public Image highlight;
    public Image image;
    public TextMeshProUGUI textMeshProUGUI;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    [SerializeField] private InventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private GameObject textBoxPrefab = null;
    [SerializeField] private int slotNumber;

    private void Awake() {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start() {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTag).transform;
    }

    private void DropSelectedItemAtMousePosition() {
        if (itemDetails != null) {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            GameObject gameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            Item item = gameObject.GetComponent<Item>();

            item.ItemCode = itemDetails.itemCode;

            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (itemDetails != null) {
            Player.Instance.DisablePlayerInputAndResetMovement();

            draggedItem = Instantiate(inventoryBar.draggedItem, inventoryBar.transform);

            Image draggedImage = draggedItem.GetComponentInChildren<Image>();
            draggedImage.sprite = image.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (draggedItem != null) {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (draggedItem != null) {
            Destroy(draggedItem);

            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null) {
                // Swap item in the inventory bar
                InventorySlot target = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();
                InventoryManager.Instance.SwapItems(InventoryLocation.player, slotNumber, target.slotNumber);
            } else {
                if (itemDetails.canBeDropped) {
                    DropSelectedItemAtMousePosition();
                }
            }

            Player.Instance.EnablePlayerInput();
            DestroyTextBox();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (itemQuantity != 0) {
            GameObject textBoxObj = Instantiate(textBoxPrefab, transform.position, Quaternion.identity);
            textBoxObj.transform.SetParent(parentCanvas.transform, false);

            inventoryBar.TextBoxGameObject = textBoxObj;

            InventoryTextbox textBoxComp = textBoxObj.GetComponent<InventoryTextbox>();

            textBoxComp.SetTextboxText(itemDetails.itemDescription, InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType), "", itemDetails.itemLongDescription, "", "");

            if (inventoryBar.IsInventoryBarPositionBottom) {
                textBoxObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                textBoxObj.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            } else {
                textBoxObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                textBoxObj.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        DestroyTextBox();
    }

    private void DestroyTextBox() {
        if (inventoryBar.TextBoxGameObject != null) {
            Destroy(inventoryBar.TextBoxGameObject);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
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
    [HideInInspector] public bool isSelected = false;

    private void Awake() {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable() {
        EventHandler.AfterSceneLoadedEvent += SceneLoaded;
    }

    private void OnDisable() {
        EventHandler.AfterSceneLoadedEvent -= SceneLoaded;
    }

    private void Start() {
        mainCamera = Camera.main;  
    }

    private void SceneLoaded() {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTag).transform;
    }

    private void DropSelectedItemAtMousePosition() {
        if (itemDetails != null) {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            GameObject gameObject = Instantiate(itemPrefab, new Vector3(worldPosition.x, worldPosition.y - Settings.gridCellSize/2f, worldPosition.z), Quaternion.identity, parentItem);
            Item item = gameObject.GetComponent<Item>();

            item.ItemCode = itemDetails.itemCode;

            if (itemQuantity == 1 && isSelected) {
                ToggleSelected();
            }

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

                if (isSelected) {
                    ToggleSelected();
                    target.ToggleSelected();
                } else if (target.isSelected) {
                    InventoryManager.Instance.SetSelectedItem(InventoryLocation.player, target.itemDetails.itemCode);
                    ShowSelectedItem(target.itemDetails);
                }
            } else {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
                // Convert to tilemap cell position
                Vector3Int cellPosition = GridPropertiesManager.Instance.grid.WorldToCell(worldPosition);

                Debug.Log($"Mouse is over tile at: {cellPosition}");
                GridPropertyDetails cellDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cellPosition.x, cellPosition.y);

                if (itemDetails.canBeDropped && cellDetails != null && cellDetails.canDropItem) {
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

    private void ShowSelectedItem(ItemDetails itemDetails) {
        if (itemDetails.canBeCarried) {
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        } else {
            Player.Instance.ClearCarriedItem();
        }
    }

    public void ToggleSelected() {
        if (isSelected) {
            highlight.color = new Color(1f, 1f, 1f, 0);
            InventoryManager.Instance.ClearSelectedItem(InventoryLocation.player);
            Player.Instance.ClearCarriedItem();
        } else if (itemQuantity > 0){
            inventoryBar.ClearItemHighlights();
            highlight.color = new Color(1f, 1f, 1f, 255);
            InventoryManager.Instance.SetSelectedItem(InventoryLocation.player, itemDetails.itemCode);
            ShowSelectedItem(itemDetails);
        }

        isSelected = !isSelected;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            ToggleSelected();
        }
    }
}

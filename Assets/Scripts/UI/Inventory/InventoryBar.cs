using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour {
    [SerializeField] private Sprite blank16x16Sprite = null;
    [SerializeField] private InventorySlot[] slots = null;
    [HideInInspector] public GameObject TextBoxGameObject = null;
    public GameObject draggedItem;

    private RectTransform rectTransform;
    private bool _isInventoryBarPositionBottom = true;
    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable() {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    private void Update() {
        SwitchInventoryBarPosition();
    }

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> list) {
        if (inventoryLocation == InventoryLocation.player) {
            for (int i = 0; i < slots.Length; i++) {
                if (i < list.Count) {
                    ItemDetails details = InventoryManager.Instance.GetItemDetail(list[i].itemCode);
                    slots[i].image.sprite = details.itemSprite;
                    slots[i].textMeshProUGUI.text = list[i].itemQuantity.ToString();
                    slots[i].itemDetails = details;
                    slots[i].itemQuantity = list[i].itemQuantity;
                } else {
                    slots[i].image.sprite = blank16x16Sprite;
                    slots[i].textMeshProUGUI.text = "";
                    slots[i].itemDetails = null;
                    slots[i].itemQuantity = 0;
                }
            }
        }
    }

    public void ClearItemHighlights() {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].highlight.color = new Color(1f, 1f, 1f, 0);
            slots[i].isSelected = false;
        }
    }

    private void SwitchInventoryBarPosition() {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();

        if (playerViewportPosition.y > 0.1f && IsInventoryBarPositionBottom == false) {
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);

            IsInventoryBarPositionBottom = true;
        }
        else if (playerViewportPosition.y <= 0.1f && IsInventoryBarPositionBottom == true) {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);

            IsInventoryBarPositionBottom = false;
        }
    }
}

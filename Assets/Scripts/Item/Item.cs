using System;
using UnityEngine;

public class Item : MonoBehaviour {
    [ItemCodeDescriptionAttr]
    [SerializeField]
    private int _itemCode;

    private SpriteRenderer spriteRenderer;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (ItemCode != 0) {
            Init(ItemCode);
        }
    }

    public void Init (int itemCodeParam) {
        if (itemCodeParam != 0) {
            ItemCode = itemCodeParam;

            ItemDetails details = InventoryManager.Instance.GetItemDetail(ItemCode);

            spriteRenderer.sprite = details.itemSprite;

            if (details.itemType == ItemType.Reapable_scenery) {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
}
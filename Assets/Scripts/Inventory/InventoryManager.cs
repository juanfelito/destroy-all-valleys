using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager> {
    private Dictionary<int, ItemDetails> itemDetailsMap;
    public List<InventoryItem>[] inventoryLists;
    [HideInInspector] public int[] inventoryListCapacity;
    public int[] selectedItem;

    [SerializeField]
    private SO_ItemList itemList = null;

    protected override void Awake() {
        base.Awake();

        CreateInventoryLists();
        CreateItemDetailsDictionary();
    }

    /// <summary>
    /// Populates the itemDetailsMap from the scriptable objects list
    /// </summary>
    private void CreateItemDetailsDictionary() {
        itemDetailsMap = new Dictionary<int, ItemDetails>();

        foreach (var item in itemList.itemDetails)
        {
            itemDetailsMap.Add(item.itemCode, item);
        }
    }

    private void CreateInventoryLists() {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++) {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacity = new int[(int)InventoryLocation.count];

        inventoryListCapacity[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;

        selectedItem = new int[(int)InventoryLocation.count];
        for (int i = 0; i < selectedItem.Length; i++) {
            selectedItem[i] = -1;
        }
    }

    /// <summary>
    /// Add an item to the inventory list for the inventoryLocation and then destroy the gameObject
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObject) {
        AddItem(inventoryLocation, item);
        Destroy(gameObject);
    }

    /// <summary>
    /// Add an item to the inventory list for the the corresponding location
    /// </summary>
    public void AddItem(InventoryLocation inventoryLocation, Item item) {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        int itemCode = item.ItemCode;

        int index = inventoryList.FindIndex(x => x.itemCode == itemCode);

        if (index != -1) {
            InventoryItem storedItem = inventoryList[index];
            storedItem.itemQuantity += 1;

            inventoryList[index] = storedItem;
        } else {
            InventoryItem itemToStore = new InventoryItem{
                itemCode = itemCode,
                itemQuantity = 1
            };

            inventoryList.Add(itemToStore);
        }

        // DebugPrintInventoryList(inventoryList);

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryList);
    }

    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode) {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int index = inventoryList.FindIndex(x => x.itemCode == itemCode);

        if (index != -1) {
            InventoryItem storedItem = inventoryList[index];
            storedItem.itemQuantity -= 1;

            if (storedItem.itemQuantity == 0) {
                inventoryList.RemoveAt(index);
            } else {
                inventoryList[index] = storedItem;
            }
        }

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryList);
    }

    public void SwapItems(InventoryLocation inventoryLocation, int from, int to) {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        if (from < inventoryList.Count && to < inventoryList.Count && from != to) {
            InventoryItem item1 = inventoryList[from];
            InventoryItem item2 = inventoryList[to];

            inventoryList[from] = item2;
            inventoryList[to] = item1;

            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryList);
        }

    }

    // private void DebugPrintInventoryList(List<InventoryItem> inventoryList) {
    //     foreach (var inventoryItem in inventoryList) {
    //         Debug.Log("Item description: " + Instance.GetItemDetail(inventoryItem.itemCode).itemDescription + " Item quantity: " + inventoryItem.itemQuantity);
    //     }
    //     Debug.Log("*********************************************************************");
    // }

    public ItemDetails GetItemDetail(int itemCode) {
        ItemDetails item;
        if (itemDetailsMap.TryGetValue(itemCode, out item)) { return item; } else { return null; }
    }

    public string GetItemTypeDescription(ItemType itemType) {
        switch (itemType){
            case ItemType.Breaking_tool:
                return Settings.BreakingTool;
            case ItemType.Chopping_tool:
                return Settings.ChoppingTool;
            case ItemType.Hoeing_tool:
                return Settings.HoeingTool;
            case ItemType.Watering_tool:
                return Settings.WateringTool;
            case ItemType.Collecting_tool:
                return Settings.CollectingTool;
            case ItemType.Reaping_tool:
                return Settings.ReapingTool;
            default:
                return itemType.ToString();
        }
    }

    public void SetSelectedItem(InventoryLocation inventoryLocation, int itemCode) {
        selectedItem[(int)inventoryLocation] = itemCode;
    }

    public void ClearSelectedItem(InventoryLocation inventoryLocation) {
        selectedItem[(int)inventoryLocation] = -1;
    }
}

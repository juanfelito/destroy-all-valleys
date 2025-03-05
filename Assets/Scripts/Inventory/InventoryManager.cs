using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager> {
    private Dictionary<int, ItemDetails> itemDetailsMap;
    public List<InventoryItem>[] inventoryLists;
    [HideInInspector] public int[] inventoryListCapacity;

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
}

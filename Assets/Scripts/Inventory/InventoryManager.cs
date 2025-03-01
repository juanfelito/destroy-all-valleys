using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager> {
    private Dictionary<int, ItemDetails> itemDetailsMap;

    [SerializeField]
    private SO_ItemList itemList = null;

    private void Start() {
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

    public ItemDetails GetItemDetail(int itemCode) {
        ItemDetails item;
        if (itemDetailsMap.TryGetValue(itemCode, out item)) { return item; } else { return null; }
    }
}

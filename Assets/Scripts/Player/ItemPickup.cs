using UnityEngine;

public class ItemPicker : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collision) {
        Item item = collision.GetComponent<Item>();

        if (item != null) {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetail(item.ItemCode);

            Debug.Log("Player is picking up a " + itemDetails.itemDescription + ": " + itemDetails.itemLongDescription);
        }
    }
}
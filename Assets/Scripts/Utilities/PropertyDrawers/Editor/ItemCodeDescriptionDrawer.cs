using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttr))]
public class ItemCodeDescriptionDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Double height to cater for the additional item code description
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.Integer) {
            EditorGUI.BeginChangeCheck();

            // Draw item code
            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height/2), label, property.intValue);

            // Draw item description
            EditorGUI.LabelField(new Rect(position.x, position.y + (position.height /2), position.width, position.height/2), "Item", GetItemDescription(property.intValue));

            // If item code value has changed, then set value to new value
            if (EditorGUI.EndChangeCheck()) {
                property.intValue = newValue;
            }
        }
        EditorGUI.EndProperty();
    }

    private string GetItemDescription(int itemCode) {
        SO_ItemList so_itemList = AssetDatabase.LoadAssetAtPath("Assets/Scriptable Objects/Item/so_ItemList.asset", typeof(SO_ItemList)) as SO_ItemList;

        List<ItemDetails> itemList = so_itemList.itemDetails;

        ItemDetails dets = itemList.Find(x => x.itemCode == itemCode);

        if (dets != null) { return dets.itemDescription; } else { return ""; }
    }
}

using System;
using System.Collections.Generic;

public delegate void MovementDelegate(MovementParameters movementParameters);

public static class EventHandler {
    // Movement event
    public static event MovementDelegate MovementEvent;

    // Movement event Call for publishers
    public static void CallMovementEvent(MovementParameters movementParameters) {
        if (MovementEvent != null) {
            MovementEvent(movementParameters);
        }
    }

    // Inventory updated event
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    // Inventory updated event Call for publishers
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList) {
        if (InventoryUpdatedEvent != null) {
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
        }
    }
}
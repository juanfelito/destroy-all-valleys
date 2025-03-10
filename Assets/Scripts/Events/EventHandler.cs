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

    // Time events

    // Advance game minute
    public static event Action<Date> AdvanceGameMinuteEvent;

    public static void CallAdvanceGameMinuteEvent(Date date) {
        if (AdvanceGameMinuteEvent != null) {
            AdvanceGameMinuteEvent(date);
        }
    }

    // Advance game hour
    public static event Action<Date> AdvanceGameHourEvent;

    public static void CallAdvanceGameHourEvent(Date date) {
        if (AdvanceGameHourEvent != null) {
            AdvanceGameHourEvent(date);
        }
    }

    // Advance game day
    public static event Action<Date> AdvanceGameDayEvent;

    public static void CallAdvanceGameDayEvent(Date date) {
        if (AdvanceGameDayEvent != null) {
            AdvanceGameDayEvent(date);
        }
    }

    // Advance game season
    public static event Action<Date> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(Date date) {
        if (AdvanceGameSeasonEvent != null) {
            AdvanceGameSeasonEvent(date);
        }
    }

    // Advance game year
    public static event Action<Date> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(Date date) {
        if (AdvanceGameYearEvent != null) {
            AdvanceGameYearEvent(date);
        }
    }
}
public enum AnimationName {
    idleDown,
    idleUp,
    idleRight,
    idleLeft,
    walkDown,
    walkUp,
    walkRight,
    walkLeft,
    runDown,
    runUp,
    runRight,
    runLeft,
    useToolDown,
    useToolUp,
    useToolRight,
    useToolLeft,
    swingToolDown,
    swingToolUp,
    swingToolRight,
    swingToolLeft,
    liftToolDown,
    liftToolUp,
    liftToolRight,
    liftToolLeft,
    holdToolDown,
    holdToolUp,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}

public enum CharacterPartAnimator {
    Body,
    Arms,
    Hair,
    Tool,
    Hat,
    Count
}

public enum PartVariantColour {
    none,
    count
}

public enum PartVariantType {
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}

public enum ToolEffect {
    none,
    watering
}

public enum Direction {
    up,
    down,
    left,
    right,
    none
}

public enum Rotation {
    Clockwise,
    Anticlockwise
}

public enum ItemType {
    Seed,
    Commodity,
    Watering_tool,
    Hoeing_tool,
    Chopping_tool,
    Breaking_tool,
    Reaping_tool,
    Collecting_tool,
    Reapable_scenery,
    Furniture,
    none,
    count
}

public enum InventoryLocation {
    player,
    chest,
    count
}

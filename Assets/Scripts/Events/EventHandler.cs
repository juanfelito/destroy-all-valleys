public delegate void MovementDelegate(MovementParameters movementParameters);

public static class EventHandler
{
    // Movement event
    public static event MovementDelegate MovementEvent;

    // Movement event Call for publishers
    public static void CallMovementEvent(MovementParameters movementParameters)
    {
        if (MovementEvent != null)
        {
            MovementEvent(movementParameters);
        }
    }
}
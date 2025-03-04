using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    // Movement Parameters
    private MovementParameters movementParameters = new MovementParameters{ toolEffect = ToolEffect.none };

    private Camera mainCamera;

    private Rigidbody2D rigidbody2D;
#pragma warning disable 414
    private Direction playerDirection;
#pragma warning restore 414

    private float movementSpeed;

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();

        rigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        #region Player Input

        ResetAnimationTriggers();
        PlayerMovementInput();
        PlayerWalkInput();

        EventHandler.CallMovementEvent(movementParameters);
        // print(movementParameters.isCarrying);
        #endregion
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 move = new Vector2(movementParameters.inputX * movementSpeed * Time.deltaTime, movementParameters.inputY * movementSpeed * Time.deltaTime);

        rigidbody2D.MovePosition(rigidbody2D.position + move);
    }

    private void ResetAnimationTriggers()
    {
        movementParameters = new MovementParameters{ toolEffect = ToolEffect.none };
    }

    private void PlayerMovementInput()
    {
        movementParameters.inputY = Input.GetAxisRaw("Vertical");
        movementParameters.inputX = Input.GetAxisRaw("Horizontal");

        if (movementParameters.inputY != 0 && movementParameters.inputX != 0)
        {
            movementParameters.inputY *= 0.71f;
            movementParameters.inputX *= 0.71f;
        }

        if (movementParameters.inputY != 0 || movementParameters.inputX != 0)
        {
            movementParameters.isRunning = true;
            movementSpeed = Settings.runningSpeed;

            // Player direction
            if (movementParameters.inputX < 0)
            {
                playerDirection = Direction.left;
            } else if (movementParameters.inputX > 0)
            {
                playerDirection = Direction.right;
            } else if (movementParameters.inputY < 0)
            {
                playerDirection = Direction.down;
            } else
            {
                playerDirection = Direction.up;
            }
        }
        else
        {
            movementParameters.isIdle = true;
        }
    }

    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementParameters.isRunning = false;
            movementParameters.isWalking = true;

            movementSpeed = Settings.walkingSpeed;
        }
    }

    public Vector3 GetPlayerViewportPosition() {
        return mainCamera.WorldToViewportPoint(transform.position);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player> {
    private AnimationOverrides animationOverrides;
    // Movement Parameters
    private MovementParameters movementParameters = new MovementParameters{ toolEffect = ToolEffect.none };

    private Camera mainCamera;

    private Rigidbody2D rigidbody2D;
#pragma warning disable 414
    private Direction playerDirection;
#pragma warning restore 414

    private List<CharacterAttribute> characterAttributesCustomizationList;
    private float movementSpeed;

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    [Tooltip("Should be populated in the prefab with the equipped item sprite renderer")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    // Player attributes that can be swapped
    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;

    protected override void Awake() {
        base.Awake();

        rigidbody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        characterAttributesCustomizationList = new List<CharacterAttribute>();

        // Initialize swappable character attributes
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Arms, PartVariantColour.none, PartVariantType.none);

        mainCamera = Camera.main;
    }

    private void Update() {
        #region Player Input

        if (!PlayerInputIsDisabled) {
            ResetAnimationTriggers();
            PlayerMovementInput();
            PlayerWalkInput();

            EventHandler.CallMovementEvent(movementParameters);
        }
        #endregion
    }

    private void FixedUpdate() {
        PlayerMovement();
    }

    private void PlayerMovement() {
        Vector2 move = new Vector2(movementParameters.inputX * movementSpeed * Time.deltaTime, movementParameters.inputY * movementSpeed * Time.deltaTime);

        rigidbody2D.MovePosition(rigidbody2D.position + move);
    }

    private void ResetAnimationTriggers() {
        movementParameters = new MovementParameters{ toolEffect = ToolEffect.none };
    }

    private void PlayerMovementInput() {
        movementParameters.inputY = Input.GetAxisRaw("Vertical");
        movementParameters.inputX = Input.GetAxisRaw("Horizontal");

        if (movementParameters.inputY != 0 && movementParameters.inputX != 0) {
            movementParameters.inputY *= 0.71f;
            movementParameters.inputX *= 0.71f;
        }

        if (movementParameters.inputY != 0 || movementParameters.inputX != 0) {
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
        else {
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

    public void DisablePlayerInputAndResetMovement() {
        DisablePlayerInput();

        movementParameters.inputX = 0f;
        movementParameters.inputY = 0f;
        movementParameters.isRunning = false;
        movementParameters.isWalking = false;
        movementParameters.isIdle = true;

        EventHandler.CallMovementEvent(movementParameters);
    }

    public void EnablePlayerInput() {
        PlayerInputIsDisabled = false;
    }

    public void DisablePlayerInput() {
        PlayerInputIsDisabled = true;
    }

    public void ShowCarriedItem(int itemCode) {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetail(itemCode);

        if (itemDetails != null) {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributesCustomizationList.Clear();
            characterAttributesCustomizationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributesCustomizationList);

            movementParameters.isCarrying = true;
        }
    }

    public void ClearCarriedItem() {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributesCustomizationList.Clear();
        characterAttributesCustomizationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributesCustomizationList);

        movementParameters.isCarrying = false;
    }

    public Vector3 GetPlayerViewportPosition() {
        return mainCamera.WorldToViewportPoint(transform.position);
    }
}

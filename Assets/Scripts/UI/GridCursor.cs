using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour {
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursor = null;
    [SerializeField] private Sprite redCursor = null;

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUseGridRadius = 0;
    public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private void OnEnable() {
        EventHandler.AfterSceneLoadedEvent += AfterSceneLoad;
    }

    private void OnDisable() {
        EventHandler.AfterSceneLoadedEvent -= AfterSceneLoad;
    }

    private void AfterSceneLoad() {
        grid = FindFirstObjectByType<Grid>();
    }

    private void Start() {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update() {
        if (CursorIsEnabled && grid != null) {
            Vector3Int cursorGridPosition = GetGridPositionForCursor();
            Vector3Int playerGridPosition = GetGridPositionForPlayer();

            // Set cursor sprite
            SetCursorValidity(cursorGridPosition, playerGridPosition);
            
            cursorRectTransform.position = GetRectTransformPositionForCursor(cursorGridPosition);
        }
    }

    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        SetCursorToValid();

        // Check item use radius is valid
        if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius
            || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius) {
            Debug.Log("1");
            SetCursorToInvalid();
            return;
        }

        GridPropertyDetails cellDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);
        if (cellDetails == null || !cellDetails.canDropItem || (SelectedItemType != ItemType.Seed && SelectedItemType != ItemType.Commodity)) {
            Debug.Log("3");
            SetCursorToInvalid();
            return;
        } 
    }

    public void DisableCursor() {
        cursorImage.color = Color.clear;
        CursorIsEnabled = false;
    }

    public void EnableCursor() {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

    private void SetCursorToInvalid() {
        cursorImage.sprite = redCursor;
        CursorPositionIsValid = false;
    }

    private void SetCursorToValid() {
        cursorImage.sprite = greenCursor;
        CursorPositionIsValid = true;
    }


    public Vector3Int GetGridPositionForCursor() {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));  // z is how far the objects are in front of the camera - camera is at -10 so objects are (-)-10 in front = 10
        return grid.WorldToCell(worldPosition);
    }

    public Vector3Int GetGridPositionForPlayer() {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition) {
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    public Vector3 GetWorldPositionForCursor() {
        return grid.CellToWorld(GetGridPositionForCursor());
    }
}

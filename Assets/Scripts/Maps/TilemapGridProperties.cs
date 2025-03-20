using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour {
    private Tilemap tilemap;
    [SerializeField] private SO_GridProperties so_gridProperties = null;
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;

    private void OnEnable() {
        if (!Application.IsPlaying(gameObject)) {
            tilemap = GetComponent<Tilemap>();

            if (so_gridProperties != null) {
                so_gridProperties.gridProperties.Clear();
            }
        }
    }

    private void OnDisable() {
        if (!Application.IsPlaying(gameObject)) {
            UpdateGridProperties();
            if (so_gridProperties != null) {
                EditorUtility.SetDirty(so_gridProperties);
            }
        }
    }

    private void UpdateGridProperties() {
        tilemap.CompressBounds();

        if (!Application.IsPlaying(gameObject))
        {
            if (so_gridProperties != null) {
                Vector3Int startCell = tilemap.cellBounds.min;
                Vector3Int endCell = tilemap.cellBounds.max;

                for (int x = startCell.x; x < endCell.x; x++) {
                    for (int y = startCell.y; y < endCell.y; y++) {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                        if (tile != null) {
                            so_gridProperties.gridProperties.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }
                    }
                }
            }
        }
    }

    private void Update() {
        if (!Application.IsPlaying(gameObject)) {
            Debug.Log("DISABLE PROPERTY TILEMAPS");
        }
    }
}

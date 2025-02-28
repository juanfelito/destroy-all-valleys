using Unity.Cinemachine;
using UnityEngine;

public class CameraConfiner : MonoBehaviour {
    void Start() {
        SwitchBoundingShape();
    }

    /// <summary>
    /// Switch the collider that cinemachine uses to define the edges of the screen
    /// </summary>
    private void SwitchBoundingShape() {
        PolygonCollider2D collider = GameObject.FindGameObjectWithTag(Tags.BoundsConfinerTag).GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();

        confiner.BoundingShape2D = collider;

        confiner.InvalidateBoundingShapeCache();
    }
}
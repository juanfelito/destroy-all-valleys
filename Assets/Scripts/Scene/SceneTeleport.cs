using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour {
    [SerializeField] private SceneName sceneNameGoTo = SceneName.Farm;
    [SerializeField] private Vector3 positionGoTo = new Vector3();

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("collided");
        if (positionGoTo.y == 0) {
            positionGoTo.y = collision.transform.position.y;
        }
        SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoTo.ToString(), positionGoTo);
    }
}
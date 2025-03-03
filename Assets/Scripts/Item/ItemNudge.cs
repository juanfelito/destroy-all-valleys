using System.Collections;
using UnityEngine;

public class ItemNudge : MonoBehaviour {
    private WaitForSeconds pause;
    private bool isAnimating = false;

    private void Awake() {
        pause = new WaitForSeconds(0.04f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!isAnimating) {
            if (transform.position.x < collision.transform.position.x) {
                StartCoroutine(Rotate(Rotation.Anticlockwise));
            } else {
                StartCoroutine(Rotate(Rotation.Clockwise));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!isAnimating) {
            if (transform.position.x > collision.transform.position.x) {
                StartCoroutine(Rotate(Rotation.Anticlockwise));
            } else {
                StartCoroutine(Rotate(Rotation.Clockwise));
            }
        }
    }

    IEnumerator Rotate(Rotation dir) {
        isAnimating = true;
        int factor = 1;
        if (dir == Rotation.Clockwise) { factor = -1; }

        for (int i = 0; i < 4; i++) {
            transform.GetChild(0).Rotate(0f, 0f, factor * 2f);
            yield return pause;
        }

        for (int i = 0; i < 5; i++) {
            transform.GetChild(0).Rotate(0f, 0f, factor * -2f);
            yield return pause;
        }

        transform.GetChild(0).Rotate(0f, 0f, factor * 2f);
        yield return pause;

        isAnimating = false;
    }
}

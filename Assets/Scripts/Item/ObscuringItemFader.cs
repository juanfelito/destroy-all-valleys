using System.Collections;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class ObscuringItemFader : MonoBehaviour {
    private SpriteRenderer sprite;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void FadeOut() {
        StartCoroutine(FadeOutRoutine());
    }

    public void FadeIn() {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeOutRoutine() {
        float currentAlpha = sprite.color.a;
        float distance = currentAlpha - Settings.targetAlpha;

        while(currentAlpha > Settings.targetAlpha) {
            currentAlpha = currentAlpha - distance/Settings.fadeOutSeconds * Time.deltaTime;
            sprite.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }

        sprite.color = new Color(1f, 1f, 1f, Settings.targetAlpha);
    }

    private IEnumerator FadeInRoutine() {
        float currentAlpha = sprite.color.a;
        float distance = 1f - currentAlpha;

        while(currentAlpha < 1f) {
            currentAlpha = currentAlpha + distance/Settings.fadeInSeconds * Time.deltaTime;
            sprite.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }

        sprite.color = new Color(1f, 1f, 1f, 1f);
    }
}
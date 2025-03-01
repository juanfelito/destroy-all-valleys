using System;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collision) {
        ObscuringItemFader[] objects = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        foreach (var fader in objects) {
            fader.FadeOut();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        ObscuringItemFader[] objects = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        foreach (var fader in objects) {
            fader.FadeIn();
        }
    }
}

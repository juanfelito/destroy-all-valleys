using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehaviour<SceneControllerManager> {
    private bool isFading;
    [SerializeField] private float fadeDuration = 0.65f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image fadeImage = null;
    public SceneName startingSceneName;

    private IEnumerator Start() {
        faderCanvasGroup.blocksRaycasts = false;
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        EventHandler.CallAfterSceneLoadedEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        StartCoroutine(FadeRoutine(0f));
    }

    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition) {
        if (!isFading) {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition) {
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        yield return StartCoroutine(FadeRoutine(1f));

        SaveLoadManager.Instance.StoreCurrentSceneData();

        Player.Instance.transform.position = spawnPosition;

        EventHandler.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventHandler.CallAfterSceneLoadedEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        vcam.OnTargetObjectWarped(Player.Instance.transform, spawnPosition - Player.Instance.transform.position);
        vcam.ForceCameraPosition(spawnPosition, Quaternion.identity);

        yield return StartCoroutine(FadeRoutine(0f));
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName) {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator FadeRoutine(float target) {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;    

        float speed = math.abs(target - faderCanvasGroup.alpha) / fadeDuration;

        while (Mathf.Abs(faderCanvasGroup.alpha - target) > 0.01f) {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, target, speed * Time.deltaTime); // Ensures smooth approach
            yield return null;
        }

        faderCanvasGroup.alpha = target;
        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }
}

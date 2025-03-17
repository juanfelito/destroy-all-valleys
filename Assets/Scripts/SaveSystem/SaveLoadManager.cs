using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager> {
    public List<ISaveable> saveableObjects;

    protected override void Awake() {
        base.Awake();

        saveableObjects = new List<ISaveable>();
    }

    public void StoreCurrentSceneData() {
        foreach (ISaveable item in saveableObjects) {
            item.StoreScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RestoreCurrentSceneData() {
        foreach (ISaveable item in saveableObjects) {
            item.RestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}
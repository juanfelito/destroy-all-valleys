using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonoBehaviour<SceneItemsManager>, ISaveable {
    private Transform parentItem;
    [SerializeField] private GameObject itemPrefab = null;

    private string _uniqueID;
    public string UniqueID { get { return _uniqueID; } set { _uniqueID = value; }}
    
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; }}

    private void AfterSceneLoad() {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTag).transform;
    }

    protected override void Awake() {
        base.Awake();

        _uniqueID = GetComponent<GenerateGUID>().GUID;
        _gameObjectSave = new  GameObjectSave();
    }

    private void OnEnable() {
        Register();
        EventHandler.AfterSceneLoadedEvent += AfterSceneLoad;
    }

    private void OnDisable() {
        Deregister();
        EventHandler.AfterSceneLoadedEvent -= AfterSceneLoad;
    }

    public void Register() {
        SaveLoadManager.Instance.saveableObjects.Add(this);
    }

    public void Deregister() {
        SaveLoadManager.Instance.saveableObjects.Remove(this);
    }

    public void RestoreScene(string sceneName) {
        Debug.Log("Restoring scene 1");
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave)) {
            Debug.Log("Restoring scene 2");
            if (sceneSave.listSceneItemDictionary != null && sceneSave.listSceneItemDictionary.TryGetValue("sceneItemsList", out List<SceneItem> sceneItems)) {
                Debug.Log("Restoring scene 3");
                DestroySceneItems();
                InstantiateSceneItems(sceneItems);
            }
        }
    }

    public void StoreScene(string sceneName) {
        Debug.Log("Storing scene");
        GameObjectSave.sceneData.Remove(sceneName);

        List<SceneItem> sceneItems = new List<SceneItem>();
        Item[] itemsInScene = FindObjectsByType<Item>(FindObjectsSortMode.None);

        foreach (Item item in itemsInScene) {
            sceneItems.Add(new SceneItem{
                itemCode = item.ItemCode,
                itemName = item.name,
                position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z)
            });
        }

        SceneSave sceneSave = new SceneSave{
            listSceneItemDictionary = new Dictionary<string, List<SceneItem>>()
        };
        sceneSave.listSceneItemDictionary.Add("sceneItemsList", sceneItems);

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    private void InstantiateSceneItems(List<SceneItem> sceneItems) {
        GameObject itemGameObject;

        foreach (SceneItem scenteItem in sceneItems) {
            itemGameObject = Instantiate(itemPrefab, new Vector3(scenteItem.position.x, scenteItem.position.y, scenteItem.position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = scenteItem.itemCode;
            item.name = scenteItem.itemName;
        }
    }

    /// <summary>
    /// Destroy items currently in the scene
    /// </summary>
    private void DestroySceneItems() {
        Item[] itemsInScene = FindObjectsByType<Item>(FindObjectsSortMode.None);

        foreach (var item in itemsInScene) {
            Destroy(item.gameObject);
        }
    }
}
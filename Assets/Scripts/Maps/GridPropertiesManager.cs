using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehaviour<GridPropertiesManager>, ISaveable {
    private string _uniqueID;
    public string UniqueID { get { return _uniqueID; } set { _uniqueID = value; }}
    
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; }}

    public Grid grid;
    private Dictionary<string, GridPropertyDetails> currentGridProperties;
    [SerializeField] private SO_GridProperties[] so_GridProperties;

    private void AfterSceneLoad() {
        grid = FindFirstObjectByType<Grid>();
    }

    protected override void Awake() {
        base.Awake();

        _uniqueID = GetComponent<GenerateGUID>().GUID;
        _gameObjectSave = new GameObjectSave();
        currentGridProperties = new Dictionary<string, GridPropertyDetails>();

        foreach (SO_GridProperties props in so_GridProperties) {
            Dictionary<string, GridPropertyDetails> dictionary = new Dictionary<string, GridPropertyDetails>();

            foreach (GridProperty item in props.gridProperties) {
                
                GridPropertyDetails details = GetGridPropertyDetails(item.coordinate.x, item.coordinate.y, dictionary);
                if (details == null) {
                    details = new GridPropertyDetails();
                }

                switch (item.property) {
                    case GridBoolProperty.canDropItem:
                        details.canDropItem = item.value;
                        break;
                    case GridBoolProperty.diggable:
                        details.isDiggable = item.value;
                        break;
                    case GridBoolProperty.canPlaceFurniture:
                        details.canPlaceFurniture = item.value;
                        break;
                    case GridBoolProperty.isPath:
                        details.isPath = item.value;
                        break;
                    case GridBoolProperty.isNPCObstacle:
                        details.isNPCObstable = item.value;
                        break;
                    default:
                        break;
                }

                SetGridPropertyDetails(item.coordinate.x, item.coordinate.y, details, dictionary);
            }

            SceneSave sceneSave = new SceneSave{
                gridPropertyDetails = dictionary
            };

            _gameObjectSave.sceneData.Add(props.sceneName.ToString(), sceneSave);
        }
    }

    private string getKey(int x, int y) {
        return x + "," + y;
    }

    public GridPropertyDetails GetGridPropertyDetails(int x, int y) {
        return GetGridPropertyDetails(x, y, currentGridProperties);
    }

    public GridPropertyDetails GetGridPropertyDetails(int x, int y, Dictionary<string, GridPropertyDetails> dict) {
        if (dict.TryGetValue(getKey(x, y), out GridPropertyDetails details)) {
            return details;
        } else {
            return null;
        }
    }

    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails details) {
        SetGridPropertyDetails(x, y, details, currentGridProperties);
    }

    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails details, Dictionary<string, GridPropertyDetails> dict) {
        details.gridX = x;
        details.gridY = y;

        dict[getKey(x,y)] = details;
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
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave)) {
            if (sceneSave.gridPropertyDetails != null) {
                currentGridProperties = sceneSave.gridPropertyDetails;
            }
        }
    }

    public void StoreScene(string sceneName) {
        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave{
            gridPropertyDetails = currentGridProperties
        };

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}

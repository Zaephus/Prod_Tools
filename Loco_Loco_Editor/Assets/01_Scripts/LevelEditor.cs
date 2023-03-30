using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    public List<Tile> tiles = new List<Tile>();

    [HideInInspector]
    public bool hasChanges;

    [SerializeField]
    private GameObject quitWarningPanel;
    [SerializeField]
    private GameObject newLevelWarningPanel;
    [SerializeField]
    private GameObject loadWarningPanel;

    private PlacementManager placementManager;
    private LevelGenerator levelGenerator;

    public void OnStart() {

        hasChanges = false;

        placementManager = GetComponent<PlacementManager>();
        placementManager.Initialize(this);

        levelGenerator = GetComponent<LevelGenerator>();

        EditorManager.TryQuit += TryQuit;

    }

    public void OnUpdate() {
        placementManager.OnUpdate();
    }

    public void TryNewLevel() {
        if(hasChanges) {
            newLevelWarningPanel.SetActive(true);
        }
        else {
            NewLevel();
        }
    }

    public void NewLevel() {
        ResetLevel();
        hasChanges = false;
    }

    public void SaveLevel() {

        if(tiles == null || tiles.Count < 1) {
            return;
        }

        Tile originTile = tiles[0];
        Vector3 size = originTile.transform.position;

        foreach(Tile t in tiles) {
            if(t.transform.position.x <= originTile.transform.position.x && t.transform.position.z <= originTile.transform.position.z) {
                originTile = t;
            }
            if(t.transform.position.x >= size.x && t.transform.position.z >= size.z) {
                size = t.transform.position;
            }
        }

        size -= originTile.transform.position;

        TileData[] tileDatas = new TileData[tiles.Count];
        for(int i = 0; i < tileDatas.Length; i++) {
            Tile t = tiles[i];
            Vector3 tilePos = t.transform.position - originTile.transform.position - size/2;
            
            tileDatas[i] = new TileData(tilePos, t.tileRotation, t.tileType, t.CurrentSwitchState, t.CurrentSwitchInputType);
        }

        DataManager.SaveLevel(tileDatas);

        hasChanges = false;

    }

    public void TryLoadLevel() {
        if(hasChanges) {
            loadWarningPanel.SetActive(true);
        }
        else {
            LoadLevel();
        }
    }

    public void LoadLevel() {
        
        TileData[] tileDatas = DataManager.LoadLevel();
        if(tileDatas == null) {
            return;
        }

        ResetLevel();

        tiles = levelGenerator.Generate(tileDatas);

        CameraMovement.CameraReset?.Invoke();

        hasChanges = false;

    }

    public void TryQuit() {
        Debug.Log("Trying to Quit");
        if(hasChanges) {
            quitWarningPanel.SetActive(true);
        }
        else {
            Quit();
        }
    }

    public void Quit() {
        EditorManager.CurrentEditorState = EditorState.Quitting;
        Application.Quit();
    }

    private void ResetLevel() {
        for(int i = tiles.Count-1; i >= 0; i--) {
            Destroy(tiles[i].gameObject);
        }
        tiles.Clear();
    }

}
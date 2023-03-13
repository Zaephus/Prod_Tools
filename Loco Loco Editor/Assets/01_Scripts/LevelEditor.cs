using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    public List<Tile> tiles = new List<Tile>();

    private PlacementManager placementManager;
    private LevelGenerator levelGenerator;

    private void Start() {
        placementManager = GetComponent<PlacementManager>();
        placementManager.Initialize(this);

        levelGenerator = GetComponent<LevelGenerator>();
    }

    public void Update() {
        placementManager.OnUpdate();
    }

    public void SaveLevel() {

        TileData[] tileDatas = new TileData[tiles.Count];
        for(int i = 0; i < tileDatas.Length; i++) {
            Tile t = tiles[i];
            tileDatas[i] = new TileData(t.transform.position, t.tileRotation, t.tileType, t.switchState, t.switchInputType);
        }

        DataManager.SaveLevel(tileDatas);

    }

    public void LoadLevel() {
        
        TileData[] tileDatas = DataManager.LoadLevel();
        if(tileDatas == null) {
            return;
        }

        for(int i = tiles.Count-1; i >= 0; i--) {
            Destroy(tiles[i].gameObject);
        }
        tiles.Clear();

        tiles = levelGenerator.Generate(tileDatas);

        CameraMovement.CameraReset.Invoke();

    }

}
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
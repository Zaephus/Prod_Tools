using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public List<Tile> Generate(TileData[] _tileDatas) {

        List<Tile> tiles = new List<Tile>();

        Vector3 tilePos;
        Vector3 tileRot;

        GameObject objectToInstantiate;

        for(int i = 0; i < _tileDatas.Length; i++) {

            objectToInstantiate = TileDatabase.Instance.GetTileByType(_tileDatas[i].tileType);

            tilePos = new Vector3(_tileDatas[i].x, 0.0f, _tileDatas[i].z);
            tileRot = objectToInstantiate.transform.eulerAngles + new Vector3(0.0f, Tile.GetTileRotation(_tileDatas[i].tileRotation), 0.0f);

            Tile tile = Instantiate(objectToInstantiate, tilePos, Quaternion.Euler(tileRot), transform).GetComponent<Tile>();

            tile.tileRotation = _tileDatas[i].tileRotation;
            tile.tileType = _tileDatas[i].tileType;
            tile.switchState = _tileDatas[i].switchState;
            tile.switchInputType = _tileDatas[i].switchInputType;

            tiles.Add(tile);

        }

        return tiles;

    }

}
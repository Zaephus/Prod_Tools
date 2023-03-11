using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public TileType tileType;
    public TileRotation tileRotation;

    public static float GetTileRotation(TileRotation _tileRotation) {
        switch(_tileRotation) {
            case TileRotation.Zero:
                return 0.0f;
            case TileRotation.One_Fourth:
                return 90.0f;
            case TileRotation.Half:
                return 180.0f;
            case TileRotation.Three_Fourth:
                return 270.0f;
            default:
                return 0.0f;
        }
    }
    
}
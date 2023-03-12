using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData {

    public int x;
    public int z;

    public TileType tileType;
    public TileRotation tileRotation;

    public SwitchState switchState;
    public SwitchInputType switchInputType;

    public TileData() {}
    public TileData(Vector3 _pos, TileRotation _rot, TileType _type) : this(_pos, _rot, _type, SwitchState.None, SwitchInputType.None) {}
    public TileData(Vector3 _pos, TileRotation _rot, TileType _type, SwitchState _switchState, SwitchInputType _inputType) {

        x = Mathf.RoundToInt(_pos.x);
        z = Mathf.RoundToInt(_pos.z);

        tileRotation = _rot;
        tileType = _type;

        switchState = _switchState;
        switchInputType = _inputType;

    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Database", menuName = "Scriptable Objects/Tile Database")]
public class TileDatabase : ScriptableSingleton<TileDatabase> {

    [Header("Tiles")]
    [SerializeField]
    private GameObject straightTrack;
    [SerializeField]
    private GameObject cornerTrack;
    [SerializeField]
    private GameObject startTrack;
    [SerializeField]
    private GameObject endTrack;
    [SerializeField]
    private GameObject tunnelTrack;
    [SerializeField]
    private GameObject switchLeftRightTrack;
    [SerializeField]
    private GameObject switchStraightLeftTrack;
    [SerializeField]
    private GameObject switchStraightRightTrack;

    [Header("Switch State Indicators")]
    [SerializeField]
    private GameObject straightIndicator;
    [SerializeField]
    private GameObject rightIndicator;
    [SerializeField]
    private GameObject leftIndicator;

    [Header("Switch Input Indicators")]
    [SerializeField]
    private GameObject greenInputIcon;
    [SerializeField]
    private GameObject yellowInputIcon;
    [SerializeField]
    private GameObject blueInputIcon;
    [SerializeField]
    private GameObject redInputIcon;

    public GameObject GetTileByType(TileType _type) {

        switch(_type) {

            case TileType.Straight:
                return straightTrack;

            case TileType.Corner:
                return cornerTrack;

            case TileType.Start:
                return startTrack;

            case TileType.End:
                return endTrack;

            case TileType.Tunnel:
                return tunnelTrack;

            case TileType.Switch_Left_Right:
                return switchLeftRightTrack;

            case TileType.Switch_Straight_Left:
                return switchStraightLeftTrack;

            case TileType.Switch_Straight_Right:
                return switchStraightRightTrack;

            default:
                return null;

        }

    }

    public GameObject[] GetAllTiles() {
        GameObject[] tiles = {
            straightTrack, 
            cornerTrack,
            startTrack,
            endTrack,
            tunnelTrack,
            switchLeftRightTrack,
            switchStraightLeftTrack,
            switchStraightRightTrack
        };
        return tiles;
    }

    public GameObject GetSwitchStateIndicator(SwitchState _state, TileType _type) {

        if(_state == SwitchState.None || _type < TileType.Switch_Left_Right) {
            return null;
        }

        switch(_type) {

            case TileType.Switch_Left_Right:
                if(_state == SwitchState.One) {
                    return rightIndicator;
                }
                else {
                    return leftIndicator;
                }

            case TileType.Switch_Straight_Left:
                if(_state == SwitchState.One) {
                    return straightIndicator;
                }
                else {
                    return leftIndicator;
                }

            case TileType.Switch_Straight_Right:
                if(_state == SwitchState.One) {
                    return straightIndicator;
                }
                else {
                    return rightIndicator;
                }

            default:
                return null;

        }
        
    }

    public GameObject GetInputIndicator(SwitchInputType _inputType) {

        switch(_inputType) {
            
            case SwitchInputType.Green:
                return greenInputIcon;

            case SwitchInputType.Yellow:
                return yellowInputIcon;

            case SwitchInputType.Blue:
                return blueInputIcon;

            case SwitchInputType.Red:
                return redInputIcon;

            default:
                return null;
        }
    }

}
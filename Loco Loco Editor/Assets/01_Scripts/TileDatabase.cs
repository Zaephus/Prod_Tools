using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Database", menuName = "Scriptable Objects/Tile Database")]
public class TileDatabase : ScriptableSingleton<TileDatabase> {

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
    private GameObject SwitchLeftRightTrack;
    [SerializeField]
    private GameObject SwitchStraightLeftTrack;
    [SerializeField]
    private GameObject SwitchStraightRightTrack;

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
                return SwitchLeftRightTrack;

            case TileType.Switch_Straight_Left:
                return SwitchStraightLeftTrack;

            case TileType.Switch_Straight_Right:
                return SwitchStraightRightTrack;

            default:
                return null;

        }

    }

}
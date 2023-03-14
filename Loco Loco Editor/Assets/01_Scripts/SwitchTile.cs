using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTile : Tile {

    [SerializeField]
    private Transform pathContainer;
    [SerializeField]
    private Transform inputIconContainer;

    private void Start() {
        ChangeStateIndicator();
        ChangeInputTypeIndicator();
    }

    protected override void ChangeStateIndicator() {
        if(pathContainer.childCount != 0) {
            Destroy(pathContainer.GetChild(0).gameObject);
        }

        if(CurrentSwitchState != SwitchState.None) {
            GameObject objectToInstantiate = TileDatabase.Instance.GetSwitchStateIndicator(CurrentSwitchState, tileType);
            Quaternion rot = Quaternion.Euler(objectToInstantiate.transform.eulerAngles + transform.eulerAngles);
            Instantiate(objectToInstantiate, pathContainer.position, rot, pathContainer);
        }
    }

    protected override void ChangeInputTypeIndicator() {
        if(inputIconContainer.childCount != 0) {
            Destroy(inputIconContainer.GetChild(0).gameObject);
        }

        if(CurrentSwitchInputType != SwitchInputType.None) {
            GameObject objectToInstantiate = TileDatabase.Instance.GetInputIndicator(CurrentSwitchInputType);
            Instantiate(objectToInstantiate, inputIconContainer.position, objectToInstantiate.transform.rotation, inputIconContainer);
        }
    }

}
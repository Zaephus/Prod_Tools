using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlacementManager : MonoBehaviour {

    private enum PlacingType {
        None = 0,
        Tiles = 1,
        SwitchState = 2,
        SwitchInput = 3
    }
    private PlacingType CurrentPlacingType {
        get {
            return placingType;
        }
        set {
            placingType = value;
            HandlePlacingTypeChange();
        }
    }
    private PlacingType placingType = PlacingType.None;

    private bool IsChecking {
        get {
            return isChecking;
        }
        set {
            isChecking = value;
            if(value) {
                tileSelector.SetActive(true);
            }
            else {
                tileSelector.SetActive(false);
            }
        }
    }
    private bool isChecking;

    [SerializeField]
    private TMP_Dropdown dropdown;
    private int dropdownValue;

    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private Color unSelectableColour;
    [SerializeField]
    private Color selectableColour;
    
    private TileRotation tileRotation = TileRotation.Zero;

    private Tile hoveredTile;

    private LevelEditor levelEditor;

    public void Initialize(LevelEditor _levelEditor) {
        levelEditor = _levelEditor;
        dropdown.gameObject.SetActive(false);

        CameraMovement.CursorLocked += ToggleChecking;
    }

    public void OnUpdate() {

        if(Input.GetKeyDown(KeyCode.R)) {
            SetTileRotation();
        }

        HandleShortcuts();

        if(IsChecking && !EventSystem.current.IsPointerOverGameObject()) {

            CheckForTile();

            if(CheckPossiblePlacement()) {
                if(Input.GetMouseButtonDown(0)) {
                    if(CurrentPlacingType == PlacingType.Tiles) {
                        PlaceTile();
                    }
                    else if(CurrentPlacingType == PlacingType.SwitchState) {
                        SetSwitchState();
                    }
                    else if(CurrentPlacingType == PlacingType.SwitchInput) {
                        SetSwitchInputType();
                    }
                }
            }

        }

    }

    public void OnDropdownValueChanged() {
        dropdownValue = dropdown.value;
    }

    public void ChangePlacingType(int _type) {
        CurrentPlacingType = (PlacingType)_type;
    }

    private void HandlePlacingTypeChange() {

        switch(CurrentPlacingType) {

            case PlacingType.Tiles:
                IsChecking = true;
                ChangeDropdownContents(typeof(TileType));
                dropdown.gameObject.SetActive(true);
                break;

            case PlacingType.SwitchState:
                IsChecking = true;
                ChangeDropdownContents(typeof(SwitchState));
                dropdown.gameObject.SetActive(true);
                break;

            case PlacingType.SwitchInput:
                IsChecking = true;
                ChangeDropdownContents(typeof(SwitchInputType));
                dropdown.gameObject.SetActive(true);
                break;

            default:
                IsChecking = false;
                dropdown.gameObject.SetActive(false);
                break;

        }
    }

    private void ChangeDropdownContents(System.Type _enumType) {

        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        for(int i = 0; i < System.Enum.GetNames(_enumType).Length; i++) {
            string dropdownName = i + " - " + System.Enum.GetNames(_enumType)[i].Replace("_", " ");
            dropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownName));
        }

        dropdown.AddOptions(dropdownOptions);

    }

    private void ToggleChecking(bool _value) {
        IsChecking = !_value;
        if(CurrentPlacingType == PlacingType.None) {
            IsChecking = false;
        }
        hoveredTile = null;
    }

    private void HandleShortcuts() {

        int tileMax = dropdown.options.Count;;
        
        if(tileMax > 10) {
            tileMax = 10;
        }

        for(int i = 0; i < tileMax; i++) {
            if(Input.GetKeyDown(i.ToString())) {
                dropdown.value = i;
            }
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) {
            levelEditor.SaveLevel();
        }

    }

    private void PlaceTile() {

        TileType type = (TileType)dropdownValue;
        
        GameObject objectToInstatiate = TileDatabase.Instance.GetTileByType(type);
        
        if(hoveredTile != null) {
            levelEditor.tiles.Remove(hoveredTile);
            Destroy(hoveredTile.gameObject);
            hoveredTile = null;
        }

        if(type != TileType.None) {
            Vector3 tilePos = tileSelector.transform.position;
            Vector3 tileRot = objectToInstatiate.transform.eulerAngles + new Vector3(0, Tile.GetTileRotation(tileRotation), 0);

            Tile tile = Instantiate(objectToInstatiate, tilePos, Quaternion.Euler(tileRot), transform).GetComponent<Tile>();
            tile.tileType = type;
            tile.tileRotation = tileRotation;

            levelEditor.tiles.Add(tile);
        }

    }

    private void SetSwitchState() {

        SwitchState state = (SwitchState)dropdownValue;

        if(hoveredTile == null) {
            return;
        }

        hoveredTile.switchState = state;

    }

    private void SetSwitchInputType() {

        SwitchInputType type = (SwitchInputType)dropdownValue;

        if(hoveredTile == null) {
            return;
        }

        hoveredTile.switchInputType = type;

    }

    private void SetTileRotation() {
        if((int)tileRotation >= System.Enum.GetValues(typeof(TileRotation)).Length - 1) {
            tileRotation = (TileRotation)0;
        }
        else {
            tileRotation++;
        }
        tileSelector.transform.eulerAngles = new Vector3(0, Tile.GetTileRotation(tileRotation), 0);
    }

    private void CheckForTile() {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit)) {

            tileSelector.transform.position = new Vector3(
                Mathf.Round(hit.point.x),
                0.0f,
                Mathf.Round(hit.point.z)
            );

            if(hit.collider.GetComponent<Tile>() != null) {
                hoveredTile = hit.collider.GetComponentInParent<Tile>();
            }
            else {
                hoveredTile = null;
            }

        }

    }

    private bool CheckPossiblePlacement() {

        if(placingType > PlacingType.Tiles) {
            if(hoveredTile == null || hoveredTile.tileType < TileType.Switch_Left_Right) {
                tileSelector.GetComponent<MeshRenderer>().material.color = unSelectableColour;
                return false;
            }
        }

        tileSelector.GetComponent<MeshRenderer>().material.color = selectableColour;
        return true;

    }

}
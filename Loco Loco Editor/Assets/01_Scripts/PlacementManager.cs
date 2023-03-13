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
    private TMP_Dropdown tileDropdown;
    private int dropdownValue;

    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private Color unSelectableColour;
    [SerializeField]
    private Color selectableColour;
    
    private TileRotation tileRotation = TileRotation.Zero;
    private TileType selectedType;
    private SwitchState selectedSwitchState;
    private SwitchInputType selectedSwitchInputType;

    private Tile hoveredTile;

    private LevelEditor levelEditor;

    public void Initialize(LevelEditor _levelEditor) {

        levelEditor = _levelEditor;
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        for(int i = 0; i < System.Enum.GetNames(typeof(TileType)).Length; i++) {
            string dropdownName = i + " - " + System.Enum.GetNames(typeof(TileType))[i].Replace("_", " ");
            dropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownName));
        }

        tileDropdown.AddOptions(dropdownOptions);

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
                    PlaceTile();
                }
            }

        }

    }

    public void OnDropdownValueChanged() {
        dropdownValue = tileDropdown.value;
    }

    public void ChangeTileSelection() {

        selectedType = (TileType)tileDropdown.value;

        
    }

    public void ChangeSwitchStateSelection() {
        
    }

    public void ChangeSwitchInputSelection() {

    }

    private void ChangeDropdownContents(System.Type _enumType) {

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        for(int i = 0; i < System.Enum.GetNames(_enumType).Length; i++) {
            string dropdownName = i + " - " + System.Enum.GetNames(_enumType)[i].Replace("_", " ");
            dropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownName));
        }

        tileDropdown.AddOptions(dropdownOptions);

    }

    private void ToggleChecking(bool _value) {
        IsChecking = !_value;
        ChangeTileSelection();
        hoveredTile = null;
    }

    private void HandleShortcuts() {

        int tileMax = tileDropdown.options.Count;;
        
        if(tileMax > 10) {
            tileMax = 10;
        }

        for(int i = 0; i < tileMax; i++) {
            if(Input.GetKeyDown(i.ToString())) {
                tileDropdown.value = i;
            }
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) {
            levelEditor.SaveLevel();
        }

    }

    private void PlaceTile() {
        
        GameObject objectToInstatiate = TileDatabase.Instance.GetTileByType(selectedType);

        Vector3 tilePos = tileSelector.transform.position;
        Vector3 tileRot = objectToInstatiate.transform.eulerAngles + new Vector3(0, Tile.GetTileRotation(tileRotation), 0);

        //int tileIndex = levelEditor.tiles.FindIndex(x => x == hoveredTile);
        //Don't know why this could be necessary
        
        if(hoveredTile != null) {
            levelEditor.tiles.Remove(hoveredTile);
            Destroy(hoveredTile.gameObject);
            hoveredTile = null;
        }

        Tile tile = Instantiate(objectToInstatiate, tilePos, Quaternion.Euler(tileRot), transform).GetComponent<Tile>();
        tile.tileType = selectedType;
        tile.tileRotation = tileRotation;

        levelEditor.tiles.Add(tile);
        //levelEditor.tiles.Insert(tileIndex, tile);

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
            if(hoveredTile.tileType < TileType.Switch_Left_Right) {
                tileSelector.GetComponent<MeshRenderer>().material.color = unSelectableColour;
                return false;
            }
        }

        tileSelector.GetComponent<MeshRenderer>().material.color = selectableColour;
        return true;

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlacementManager : MonoBehaviour {

    private enum PlacingType {
        None,
        PlacingTiles,
        Bulldozing
    }
    private PlacingType CurrentPlacingType {
        get {
            return placingType;
        }
        set {
            if(selector != null) {
                selector.SetActive(false);
            }

            placingType = value;

            if(placingType == PlacingType.PlacingTiles) {
                selector = tileSelector;
                selector.SetActive(true);
            }
            else if(placingType == PlacingType.Bulldozing) {
                selector = bulldozeSelector;
                selector.SetActive(true);
            }

        }
    }
    private PlacingType placingType = PlacingType.None;

    private bool IsChecking {
        get {
            return isChecking;
        }
        set {
            isChecking = value;
        }
    }
    private bool isChecking = true;

    private TileType tileType;

    [SerializeField]
    private Toggle bulldozeToggle;

    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private GameObject bulldozeSelector;

    private GameObject selector;
    
    private TileRotation tileRotation = TileRotation.Zero;

    private Tile hoveredTile;

    private LevelEditor levelEditor;

    public void Initialize(LevelEditor _levelEditor) {
        levelEditor = _levelEditor;

        CameraMovement.CursorLocked += ToggleChecking;
        InventoryItem.ItemSelected += ChangeTileType;
    }

    public void OnUpdate() {

        HandleShortcuts();

        if(IsChecking && !EventSystem.current.IsPointerOverGameObject()) {

            CheckForTile();

            if(CurrentPlacingType == PlacingType.PlacingTiles || CurrentPlacingType == PlacingType.Bulldozing) {
                if(Input.GetMouseButtonDown(0)) {
                    PlaceTile();
                }
            }

        }

    }

    public void ChangeTileType(int _tileType) {
        if(CurrentPlacingType == PlacingType.Bulldozing) {
            bulldozeToggle.isOn = !bulldozeToggle.isOn;
        }

        if(tileType == (TileType)_tileType) {
            CurrentPlacingType = PlacingType.None;
        }
        else {
            tileType = (TileType)_tileType;
            CurrentPlacingType = PlacingType.PlacingTiles;
        }
    }

    public void ToggleBulldozing() {
        if(CurrentPlacingType == PlacingType.Bulldozing) {
            CurrentPlacingType = PlacingType.None;
        }
        else {
            CurrentPlacingType = PlacingType.Bulldozing;
            tileRotation = TileRotation.Zero;
            tileType = TileType.None;
        }
    }

    private void ToggleChecking(bool _value) {
        IsChecking = !_value;
        hoveredTile = null;
    }

    private void HandleShortcuts() {

        if(CurrentPlacingType == PlacingType.PlacingTiles && Input.GetKeyDown(KeyCode.R)) {
            SetTileRotation();
        }

        if(Input.GetMouseButtonDown(1)) {
            if(CurrentPlacingType == PlacingType.PlacingTiles) {
                ChangeTileType((int)tileType);
            }
            if(CurrentPlacingType == PlacingType.Bulldozing) {
                bulldozeToggle.isOn = !bulldozeToggle.isOn;
            }
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) {
            levelEditor.SaveLevel();
        }

    }

    private void PlaceTile() {
        
        GameObject objectToInstatiate = TileDatabase.Instance.GetTileByType(tileType);
        
        if(hoveredTile != null) {
            levelEditor.tiles.Remove(hoveredTile);
            Destroy(hoveredTile.gameObject);
            hoveredTile = null;
        }

        if(tileType != TileType.None) {
            Vector3 tilePos = tileSelector.transform.position;
            Vector3 tileRot = objectToInstatiate.transform.eulerAngles + new Vector3(0, Tile.GetTileRotation(tileRotation), 0);

            Tile tile = Instantiate(objectToInstatiate, tilePos, Quaternion.Euler(tileRot), transform).GetComponent<Tile>();
            tile.tileType = tileType;
            tile.tileRotation = tileRotation;

            if(tileType >= TileType.Switch_Left_Right) {
                tile.CurrentSwitchState = SwitchState.One;
                tile.CurrentSwitchInputType = SwitchInputType.Green;
            }

            levelEditor.tiles.Add(tile);
        }

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

            if(selector != null) {
                selector.transform.position = new Vector3(
                    Mathf.Round(hit.point.x),
                    0.0f,
                    Mathf.Round(hit.point.z)
                );
            }

            if(hit.collider.GetComponent<Tile>() != null) {
                hoveredTile = hit.collider.GetComponentInParent<Tile>();
            }
            else {
                hoveredTile = null;
            }

        }

    }

}
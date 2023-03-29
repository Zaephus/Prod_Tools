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
        Bulldozing,
        SwitchEditing
    }
    private PlacingType CurrentPlacingType {
        get {
            return placingType;
        }
        set {
            
            placingType = value;

            if(selector != null) {
                selector.SetActive(false);
            }
            switchEditingSelector.SetActive(false);

            if(placingType != PlacingType.SwitchEditing) {
                switchContextMenu.gameObject.SetActive(false);
            }

            if(placingType == PlacingType.PlacingTiles) {
                if(tileSelector.transform.childCount > 0) {
                    for(int i = tileSelector.transform.childCount-1; i >= 0; i--) {
                        Destroy(tileSelector.transform.GetChild(i).gameObject);
                    }
                }
                GameObject preview = Instantiate(TileDatabase.Instance.GetTileByType(tileType), tileSelector.transform.position, tileSelector.transform.rotation, tileSelector.transform);
                preview.GetComponent<Collider>().enabled = false;

                selector = tileSelector;
                selector.SetActive(true);
            }
            else if(placingType == PlacingType.Bulldozing) {
                selector = bulldozeSelector;
                selector.SetActive(true);
            }
            else if(placingType == PlacingType.SwitchEditing) {
                switchEditingSelector.SetActive(true);
                switchContextMenu.gameObject.SetActive(true);
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
    private SwitchContextMenu switchContextMenu;

    [SerializeField]
    private GameObject tileSelector;
    [SerializeField]
    private GameObject bulldozeSelector;
    [SerializeField]
    private GameObject switchEditingSelector;
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

            if(Input.GetMouseButtonDown(0)) {
                if(CurrentPlacingType == PlacingType.PlacingTiles || CurrentPlacingType == PlacingType.Bulldozing) {
                    PlaceTile();
                }
                else if(CurrentPlacingType == PlacingType.None || CurrentPlacingType == PlacingType.SwitchEditing) {
                    if(hoveredTile != null) {
                        if(hoveredTile.tileType >= TileType.Switch_Left_Right) {
                            switchEditingSelector.transform.position = hoveredTile.transform.position;
                            switchContextMenu.Initialize((SwitchTile)hoveredTile);
                            CurrentPlacingType = PlacingType.SwitchEditing;
                        }
                    }
                    else {
                        CurrentPlacingType = PlacingType.None;
                    }
                }
            }

        }

    }

    public void ChangeTileType(int _tileType) {
        if(CurrentPlacingType == PlacingType.Bulldozing) {
            bulldozeToggle.isOn = !bulldozeToggle.isOn;
        }

        if(tileType == (TileType)_tileType) {
            tileType = TileType.None;
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

        if(CurrentPlacingType == PlacingType.PlacingTiles) {
            if(Input.GetKeyDown(KeyCode.R)) {
                if(Input.GetKey(KeyCode.LeftShift)) {
                    SetTileRotation(-1);
                }
                else {
                    SetTileRotation(1);
                }
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            if(CurrentPlacingType == PlacingType.PlacingTiles) {
                ChangeTileType((int)tileType);
            }
            if(CurrentPlacingType == PlacingType.Bulldozing) {
                bulldozeToggle.isOn = !bulldozeToggle.isOn;
            }
            if(CurrentPlacingType == PlacingType.SwitchEditing) {
                CurrentPlacingType = PlacingType.None;
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

    private void SetTileRotation(int _dir) {
        if(_dir > 0) {
            if((int)tileRotation >= System.Enum.GetValues(typeof(TileRotation)).Length - 1) {
                tileRotation = (TileRotation)0;
            }
            else {
                tileRotation++;
            }
        }
        else if(_dir < 0) {
            if((int)tileRotation <= 0) {
                tileRotation = (TileRotation)(System.Enum.GetValues(typeof(TileRotation)).Length - 1);
            }
            else {
                tileRotation--;
            }
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
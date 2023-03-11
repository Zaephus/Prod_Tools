using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlacementManager : MonoBehaviour {

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

    [SerializeField]
    private GameObject tileSelector;
    
    private TileType selectedType;
    private TileRotation tileRotation = TileRotation.Zero;

    private Tile hoveredTile;

    private LevelEditor levelEditor;

    public void Initialize(LevelEditor _levelEditor) {
        levelEditor = _levelEditor;
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        for(int i = 0; i < System.Enum.GetNames(typeof(TileType)).Length; i++) {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(System.Enum.GetNames(typeof(TileType))[i]));
        }

        tileDropdown.AddOptions(dropdownOptions);
    }

    public void OnUpdate() {

        if(Input.GetKeyDown(KeyCode.R)) {
            SetTileRotation();
        }

        if(IsChecking && !EventSystem.current.IsPointerOverGameObject()) {

            CheckForTile();

            if(Input.GetMouseButtonDown(0)) {
                if(selectedType != TileType.None) {
                    PlaceTile();
                }
            }

        }

    }

    public void ChangeSelection() {

        selectedType = (TileType)tileDropdown.value;

        if(selectedType == TileType.None) {
            IsChecking = false;
        }
        else {
            IsChecking = true;
        }

    }

    private void PlaceTile() {
        
        GameObject objectToInstatiate = TileDatabase.Instance.GetTileByType(selectedType);

        Vector3 tilePos = tileSelector.transform.position;
        Vector3 tileRot = objectToInstatiate.transform.eulerAngles + new Vector3(0, Tile.GetTileRotation(tileRotation), 0);

        //int tileIndex = levelEditor.tiles.FindIndex(x => x == hoveredTile);
        //Don't know why this could be necessary

        Debug.Log(hoveredTile);
        
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

}
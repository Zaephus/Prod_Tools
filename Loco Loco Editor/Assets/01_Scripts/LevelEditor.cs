using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    public List<Tile> tiles = new List<Tile>();

    private PlacementManager placementManager;

    private void Start() {
        placementManager = GetComponent<PlacementManager>();
        placementManager.Initialize(this);
    }

    public void Update() {
        placementManager.OnUpdate();
    }

    public void SaveLevel() {

    }

    public void LoadLevel() {
        
    }

}
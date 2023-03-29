using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject tilePreviewPrefab;
    [SerializeField]
    private GameObject offsetTilePreviewPrefab;

    [SerializeField]
    private Vector3 positionOffset;
    [SerializeField]
    private Vector3 rotationOffset;

    public TileType[] offsetExceptions;

    public RenderTexture[] GeneratePreviews() {

        GameObject[] tiles = TileDatabase.Instance.GetAllTiles();
        RenderTexture[] renderTextures = new RenderTexture[tiles.Length];

        GameObject objectToInstantiate;

        for(int i = 0; i < tiles.Length; i++) {

            objectToInstantiate = tilePreviewPrefab;

            Vector3 pos = transform.position + new Vector3(i * 2, 0, 0) + positionOffset;
            Vector3 rot = objectToInstantiate.transform.eulerAngles + rotationOffset - new Vector3(0, 90, 0);

            for(int j = 0; j < offsetExceptions.Length; j++) {
                if((int)offsetExceptions[j] == i + 1) {
                    objectToInstantiate = offsetTilePreviewPrefab;
                    rot = objectToInstantiate.transform.eulerAngles + rotationOffset;
                    break;
                }
            }

            GameObject tilePreview = Instantiate(objectToInstantiate, pos, Quaternion.identity, transform);
            GameObject tile = Instantiate(tiles[i], pos, Quaternion.Euler(rot), tilePreview.transform);
            tile.layer = LayerMask.NameToLayer("Tile Preview");
            for(int k = 0; k < tile.transform.childCount; k++) {
                tile.transform.GetChild(k).gameObject.layer = LayerMask.NameToLayer("Tile Preview");
            }

            renderTextures[i] = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            renderTextures[i].Create();

            Camera previewCamera = tilePreview.GetComponentInChildren<Camera>();
            previewCamera.targetTexture = renderTextures[i];
        }

        return renderTextures;
        
    }

}
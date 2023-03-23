using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    [SerializeField]
    private PreviewGenerator previewGenerator;

    [SerializeField]
    private GameObject inventoryItemPrefab;

    [SerializeField]
    private GameObject inventoryItemContainer;

    private void Start() {

        RenderTexture[] renderTextures = previewGenerator.GeneratePreviews();
        
        for(int i = 0; i < renderTextures.Length; i++) {
            InventoryItem item = Instantiate(inventoryItemPrefab, inventoryItemContainer.transform).GetComponent<InventoryItem>();
            item.tileType = i + 1;
            item.GetComponent<RawImage>().texture = renderTextures[i];
        }

    }
}

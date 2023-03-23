using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    public static System.Action<int> ItemSelected;

    [HideInInspector]
    public int tileType;

    public void Select() {
        ItemSelected?.Invoke(tileType);
    }

}
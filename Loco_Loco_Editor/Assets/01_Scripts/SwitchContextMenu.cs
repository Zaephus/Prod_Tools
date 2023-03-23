using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchContextMenu : MonoBehaviour {

    [SerializeField]
    private TMP_Text nameText;

    [Header("Switch State")]
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private Slider stateSlider;

    [Header("Switch Input Type")]
    [SerializeField]
    private TMP_Text inputTypeText;
    [SerializeField]
    private Slider inputTypeSlider;


}
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

    private SwitchTile switchTile;

    private LevelEditor levelEditor;

    public void OnStart(LevelEditor _levelEditor) {
        levelEditor = _levelEditor;
    }

    public void Initialize(SwitchTile _switchTile) {
        switchTile = _switchTile;

        nameText.text = switchTile.tileType.ToString().Replace("_", " ");
        stateSlider.value = (float)switchTile.CurrentSwitchState;
        inputTypeSlider.value = (float)switchTile.CurrentSwitchInputType;
    }

    public void OnStateSliderValueChanged() {
        SwitchState state = (SwitchState)stateSlider.value;
        stateText.text = "Switch State: " + state.ToString();
        switchTile.CurrentSwitchState = state;
        levelEditor.hasChanges = true;
    }

    public void OnInputTypeSliderValueChanged() {
        SwitchInputType inputType = (SwitchInputType)inputTypeSlider.value;
        inputTypeText.text = "Input Type: " + inputType.ToString();
        switchTile.CurrentSwitchInputType = inputType;
        levelEditor.hasChanges = true;
    }

}
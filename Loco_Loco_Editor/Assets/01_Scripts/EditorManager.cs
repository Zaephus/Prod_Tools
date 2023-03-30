using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour {

    public static System.Action TryQuit;
    private static System.Action StateChanged;

    public static EditorState CurrentEditorState {
        get {
            return editorState;
        }
        set {
            editorState = value;
            StateChanged?.Invoke();
        }
    }
    private static EditorState editorState;

    private static bool hasChanges;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private LevelEditor levelEditor;

    private void Start() {
        StateChanged += HandleStateChanges;
        CurrentEditorState = EditorState.MainMenu;
    }

    private void Update() {
        if(CurrentEditorState == EditorState.Editor) {
            levelEditor.OnUpdate();
            hasChanges = levelEditor.hasChanges;
        }
    }
    
    public void StartNewLevel() {
        CurrentEditorState = EditorState.Editor;
    }

    public void StartLoadLevel() {
        CurrentEditorState = EditorState.Editor;
        levelEditor.TryLoadLevel();
    }

    private void HandleStateChanges() {
        switch(CurrentEditorState) {
            case EditorState.MainMenu:
                levelEditor.gameObject.SetActive(false);
                mainMenu.SetActive(true);
                break;

            case EditorState.Editor:
                mainMenu.SetActive(false);
                levelEditor.gameObject.SetActive(true);
                levelEditor.OnStart();
                break;
        }
    }

    private static bool WantsToQuit() {
        if(CurrentEditorState == EditorState.Editor && hasChanges) {
            TryQuit?.Invoke();
            return false;
        }
        else {
            return true;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void RunOnStart() {
        Application.wantsToQuit += WantsToQuit;
    }

}
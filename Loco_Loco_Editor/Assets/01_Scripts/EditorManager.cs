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

    [SerializeField]
    private LevelEditor levelEditor;

    private void Start() {
        StateChanged += HandleStateChanges;
        CurrentEditorState = EditorState.Editor;

        levelEditor.OnStart();
    }

    private void Update() {
        levelEditor.OnUpdate();
    }

    private static void HandleStateChanges() {

    }

    private static bool WantsToQuit() {
        if(CurrentEditorState == EditorState.Editor) {
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour {

    public static System.Action<bool> CursorLocked;
    public static System.Action CameraReset;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject cameraRotator;

    private Vector3 mouseDelta;
    
    private float scrollDelta;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startZoom;

    private void Start() {
        startPosition = transform.position;
        startRotation = cameraRotator.transform.rotation;
        startZoom = mainCamera.orthographicSize;
        CameraReset += ResetCamera;
    }

    private void Update() {

        mouseDelta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        mouseDelta.Normalize();

        scrollDelta = Input.mouseScrollDelta.y;

        if(Input.GetMouseButton(2)) {
            HideAndLockCursor();
            Vector3 moveDir = mouseDelta.x * cameraRotator.transform.right + mouseDelta.z * cameraRotator.transform.forward;
            float moveModifier = mainCamera.orthographicSize * (0.6f + 0.4f * (1/(mainCamera.orthographicSize/2)));
            transform.position += moveDir * moveSpeed * moveModifier * Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(2)) {
            ShowAndUnlockCursor();
        }

        if(Input.GetKey(KeyCode.LeftAlt)) {
            HideAndLockCursor();
            if(Input.GetMouseButton(0)) {
                cameraRotator.transform.eulerAngles -= new Vector3(0, mouseDelta.x, 0) * rotateSpeed * Time.deltaTime;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt)) {
            ShowAndUnlockCursor();
        }

        if(!EventSystem.current.IsPointerOverGameObject()) {
            if(scrollDelta < 0.0f && mainCamera.orthographicSize <= 20) {
                mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
            }
            if(scrollDelta > 0.0f && mainCamera.orthographicSize >= 2) {
                mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
            }
        }

        if(mainCamera.orthographicSize > 20) {
            mainCamera.orthographicSize = 20;
        }
        if(mainCamera.orthographicSize < 2) {
            mainCamera.orthographicSize = 2;
        }

    }

    private void ResetCamera() {
        transform.position = startPosition;
        cameraRotator.transform.rotation = startRotation;
        mainCamera.orthographicSize = startZoom;
    }

    private void ShowAndUnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CursorLocked?.Invoke(false);
    }

    private void HideAndLockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CursorLocked?.Invoke(true);
    }

}
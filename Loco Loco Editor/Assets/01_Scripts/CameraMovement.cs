using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private Camera mainCamera;

    private float horizontal;
    private float vertical;
    
    private float scrollDelta;

    private void Update() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        scrollDelta = Input.mouseScrollDelta.y;

        Vector3 velocity = new Vector3(horizontal, 0, vertical);
        velocity.Normalize();

        transform.position += velocity * moveSpeed * Time.deltaTime;

        if(scrollDelta < 0.0f && mainCamera.orthographicSize <= 20) {
            mainCamera.orthographicSize += zoomSpeed;
        }
        if(scrollDelta > 0.0f && mainCamera.orthographicSize >= 2) {
            mainCamera.orthographicSize -= zoomSpeed;
        }
        
    }
}
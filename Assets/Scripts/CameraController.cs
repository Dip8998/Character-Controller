using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float distanceCamera;
    [SerializeField] float minVerticalAngle;
    [SerializeField] float maxVerticalAngle;
    [SerializeField] float speedCamera;
    [SerializeField] Vector2 framingOffset;

    float rotationX;
    float rotationY;
    
    private void Start()
    {
        CursorLocked();
    }
    void Update()
    {
        RotateAndFocus();
    }

    private static void CursorLocked()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void RotateAndFocus()
    {
        // Use for x mouse rotation
        rotationX += Input.GetAxis("Mouse Y") * speedCamera;
        // Use for x vertical value
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        // Use for y mouse rotation
        rotationY += Input.GetAxis("Mouse X") * speedCamera;
        // Use for focus on player
        var focusPosition = Player.transform.position + new Vector3(framingOffset.x, framingOffset.y);
        // Use for exceessing the rotation
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        // Use for position of the camera
        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distanceCamera);
        // Use for focusing the player while rotation
        transform.rotation = targetRotation;
    }

    // this use for player do not move in y axis while movement
     public Quaternion planerRotation => Quaternion.Euler(0,rotationY, 0);
}

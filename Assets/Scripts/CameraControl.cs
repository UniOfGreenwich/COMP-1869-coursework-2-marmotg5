using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

enum CameraState
{
    CAMERA_LOCKED,
    CAMERA_FREEMODE
}

public class CameraControl : MonoBehaviour
{
    Camera mainCamera;

    [Header("Keyboard Controls")]
    [SerializeField]
    KeyCode moveCamForwardsKey = KeyCode.W;
    [SerializeField]
    KeyCode moveCamLeftKey = KeyCode.A;
    [SerializeField]
    KeyCode moveCamBackwardsKey = KeyCode.S;
    [SerializeField]
    KeyCode moveCamRightKey = KeyCode.D;
    [SerializeField]
    KeyCode toggleCameraLockKey = KeyCode.Space;

    [Header("Mouse Controls")]
    [SerializeField]
    MouseButton freeroamCameraRotateButton = MouseButton.RightMouse;

    [Header("Camera Zoom")]
    [SerializeField]
    float minZoomDistance = 20.0f;
    [SerializeField]
    float maxZoomDistance = 40.0f;
    [SerializeField]
    float zoomAmountPerScroll = 5.0f;

    [Header("Camera Lock Settings")]
    float cameraLockDistanceRay = 500.0f;
    [SerializeField]
    float cameraRotatingSpeed = 10.0f;

    [SerializeField]
    Vector3 offsetCoords = Vector3.zero; // The coords where the camera will be rotating/looking at (realistically we want at 0,0,0)
    Vector3 pointingCoords;

    [SerializeField]
    CameraState cameraState = CameraState.CAMERA_LOCKED; // If the camera is locked, it will only be able to move around the offsetCoords

    [Header("Camera Freeroam Settings")]
    [SerializeField]
    float mouseSensitivity = 10.0f;
    [SerializeField]
    float cameraAcceleration = 5.0f;
    [SerializeField]
    float cameraDeceleration = 15.0f;
    [SerializeField]
    float cameraMaxAcceleration = 7.5f;
    float currentCameraAcceleration = 0.0f;
    [SerializeField]
    float cameraMovingSpeed = 7.5f;
    [SerializeField]
    GameObject cameraFreeroamBox; // The box where the camera will have to stay within in freeroam mode (can't leave/exit it)
    BoxCollider cameraFreeroamBoxCollider = null;

    void Start()
    {
        mainCamera = Camera.main;
        pointingCoords = offsetCoords;

        if (cameraFreeroamBox)
        {
            cameraFreeroamBoxCollider = cameraFreeroamBox.GetComponent<BoxCollider>();
        }
    }

    void Update()
    {
        HandleCameraControl();
        HandleCameraZoom();
        HandleCameraLock();

		if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                print(raycastHit.point);
            }
		}
	}

    void HandleCameraControl()
    {
        if (cameraState == CameraState.CAMERA_LOCKED)
        {
            // Force the camera to look at the pointing coords at all times
            mainCamera.transform.LookAt(pointingCoords, Vector3.up);

            // Move camera around offset coords
            if (Input.GetKey(moveCamLeftKey))
            {
                mainCamera.transform.RotateAround(pointingCoords, Vector3.up, cameraRotatingSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(moveCamRightKey))
            {
                mainCamera.transform.RotateAround(pointingCoords, Vector3.down, cameraRotatingSpeed * Time.deltaTime);
            }
        }
        else if (cameraState == CameraState.CAMERA_FREEMODE)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // A-D
            float verticalInput = Input.GetAxis("Vertical"); // W-S
           
            Vector3 cameraDirection = new Vector3(mainCamera.transform.forward.x * horizontalInput, 0.0f, mainCamera.transform.forward.z * verticalInput); // Ignore Y - we don't want to be able to go up/down (only zoom)
            Vector3 moveDir = mainCamera.transform.right * horizontalInput + mainCamera.transform.forward * verticalInput;
            moveDir.y = 0.0f;

            // Check if we are holding right-click to rotate camera horizontally
            if (Input.GetMouseButton((int)freeroamCameraRotateButton))
            {
                float mouseX = Input.GetAxis("Mouse X");
                if (mouseX != 0)
                {
                    Vector3 cameraRotation = new Vector3(0.0f, mouseX * mouseSensitivity, 0.0f); // Camera uses Y to rotate horizontally
                    mainCamera.transform.eulerAngles += cameraRotation;
                }
            }

            // We start moving the camera in freeroam state using player input
            if (horizontalInput != 0 || verticalInput != 0)
            {
                currentCameraAcceleration = Mathf.Min(currentCameraAcceleration + cameraAcceleration * Time.deltaTime, cameraMaxAcceleration);
            }
            else
            {
                currentCameraAcceleration = Mathf.Max(currentCameraAcceleration - cameraDeceleration * Time.deltaTime, 0.0f);
            }

            mainCamera.transform.position += moveDir * cameraMovingSpeed * currentCameraAcceleration * Time.deltaTime;

            //print("Are coords in barrier: " + AreCoordsInBox(mainCamera.transform.position, cameraFreeroamBoxCollider));
            //print("Camera bounding box max bounds coords: " + cameraFreeroamBoxCollider.bounds.max);
            //print("Camera bounding box min bounds coords: " + cameraFreeroamBoxCollider.bounds.min);
        }
    }

    void HandleCameraZoom()
    {
        Vector2 mouseScrollDelta = Input.mouseScrollDelta; // Scroll wheel value
        if (mouseScrollDelta.y != 0)
        {
            Vector3 direction = (offsetCoords - mainCamera.transform.position).normalized;
            Vector3 newCameraPosition;

            if (mouseScrollDelta.y > 0) // Zooming in
            {
                newCameraPosition = mainCamera.transform.position + (direction * zoomAmountPerScroll);

            }
            else // Zooming out
            {
                newCameraPosition = mainCamera.transform.position + (-direction * zoomAmountPerScroll);
            }

            // Making sure that we are keeping the distance from the camera to the offset limited within the min/max zoom distances
            float yDistance = Mathf.Abs(newCameraPosition.y - offsetCoords.y);
            if (yDistance < minZoomDistance || yDistance > maxZoomDistance) return;

            // Try to clamp the y-value of the new position in case it exceeds the min/max zoom distances
            newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, minZoomDistance, maxZoomDistance);

            mainCamera.transform.position = newCameraPosition;
        }
    }

    void HandleCameraLock()
    {
        if (Input.GetKeyDown(toggleCameraLockKey))
        {
            if (cameraState == CameraState.CAMERA_LOCKED)
            {
                SetCameraState(CameraState.CAMERA_FREEMODE);
            }
            else if (cameraState == CameraState.CAMERA_FREEMODE)
            {
                Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0.0f);
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, cameraLockDistanceRay))
                {
                    // Get the coords
                    Vector3 hitpoint = hit.point;

                    pointingCoords = hitpoint;
                }

                SetCameraState(CameraState.CAMERA_LOCKED);
            }
        }
    }

    void SetCameraState(CameraState state)
    {
        cameraState = state;
    }

    // Gonna use this to make sure the camera stays within its bounding box when in free-roam mode
    bool AreCoordsInBox(Vector3 coordsToCheck, Collider colliderToCheck = null)
    {
        if (colliderToCheck)
        {
            Bounds bounds = colliderToCheck.bounds;

            // Check if our coords are within the min/max of the bounds of the box
            if (coordsToCheck.x >= bounds.min.x &&
                coordsToCheck.x <= bounds.max.x &&
                coordsToCheck.y >= bounds.min.y &&
                coordsToCheck.y <= bounds.max.y &&
                coordsToCheck.z >= bounds.min.z &&
                coordsToCheck.z <= bounds.max.z)
            {
                return true;
            }
        }

        return false;
    }
}

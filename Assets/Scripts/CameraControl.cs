using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    Vector3 pointingCoords = Vector3.zero; // The coords where the camera will be rotating/looking at | default: (0,0,0)

	[SerializeField]
    CameraState cameraState = CameraState.CAMERA_LOCKED; // If the camera is locked, it will only be able to move around the offsetCoords

    [Header("Camera Freeroam Settings")]
    [SerializeField]
    float mouseSensitivity = 0.5f;
    [SerializeField]
    float cameraAcceleration = 2.5f;
    [SerializeField]
    float cameraDeceleration = 15.0f;
    [SerializeField]
    float cameraMaxAcceleration = 5.0f;
    float currentCameraAcceleration = 0.0f;
    [SerializeField]
    float cameraMovingSpeed = 5.0f;
    [SerializeField]
    GameObject cameraFreeroamBox; // The box where the camera will have to stay within in freeroam mode (can't leave/exit it)
    BoxCollider cameraFreeroamBoxCollider = null;

    // Phone swipe data (stores data for the phone inputs done by players)
    Vector2 leftTouchScreenStartPosition = Vector2.zero;
    int leftSideFingerID = -1;
    Vector2 rightTouchScreenStartPosition = Vector2.zero;
    int rightSideFingerID = -1;

    void Start()
    {
        mainCamera = Camera.main;

        if (cameraFreeroamBox)
        {
            cameraFreeroamBoxCollider = cameraFreeroamBox.GetComponent<BoxCollider>();
        }
    }

    void Update()
    {
        UpdateCameraPointingCoords();

        ApplyPhoneControls();
        HandleCameraControl();
        HandleCameraZoom();
        HandleCameraLock();
	}

    void UpdateCameraPointingCoords()
    {
        Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0.0f);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, cameraLockDistanceRay))
        {
            // Get the coords
            Vector3 hitpoint = hit.point;

            pointingCoords = hitpoint;
        }
    }

    void ApplyPhoneControls()
    {
        if (Input.touches.Length >= 1)
        {
            // Handle the rest of the touches on the screen
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    // Ignore the phone touch and the rest of the code if the player is clicking/moving any UI elements
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                    // Check for the device orienation (using the same functions after determening which orientation to phone is in
                    // because the Screen.width changes which is what's checked in the functions
                    if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
                    {
                        // Do some check to see which side of the screen the touch has been made on
                        ApplyLeftSidePhoneTouch(touch);
                        ApplyRightSidePhoneTouch(touch);
                    }
                    else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
                    {
                        // Do some check to see which side of the screen the touch has been made on
                        ApplyLeftSidePhoneTouch(touch);
                        ApplyRightSidePhoneTouch(touch);
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Checks first if the player is trying to pinch their screen
                    //if (ApplyPhonePinch()) return; // Ignore the rest of the function, since player is only trying to zoom in/out the camera

                    // Left side finger detected -> move the camera with the player's input
                    if (leftSideFingerID == touch.fingerId)
                    {
                        // Get the touch screen swipe movement direction
                        Vector2 leftTouchScreenMoveDirection = (leftTouchScreenStartPosition - touch.position).normalized; // (touch.position - leftTouchScreenStartPosition ).normalized; // Goes where the player swipes towards

                        //Vector3 moveDir = mainCamera.transform.right * leftTouchScreenMoveDirection.x + mainCamera.transform.forward * leftTouchScreenMoveDirection.y;
                        Vector3 moveDir = transform.right * leftTouchScreenMoveDirection.x +  transform.forward * leftTouchScreenMoveDirection.y;
                        moveDir.y = 0.0f;

                        currentCameraAcceleration = Mathf.Min(currentCameraAcceleration + cameraAcceleration * Time.deltaTime, cameraMaxAcceleration);

                        mainCamera.transform.position += moveDir * cameraMovingSpeed * currentCameraAcceleration * Time.deltaTime;
                    }
                    // Rotate the camera -> right side finger is detected
                    else if (rightSideFingerID == touch.fingerId)
                    {
                        Vector2 rightTouchScreenMoveDirection = (rightTouchScreenStartPosition - touch.position).normalized;

                        Vector3 cameraRotation = new Vector3(0.0f, rightTouchScreenMoveDirection.x * mouseSensitivity, 0.0f); // Camera uses Y to rotate horizontally
                        mainCamera.transform.eulerAngles += cameraRotation;
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (leftSideFingerID == touch.fingerId)
                    {
                        leftSideFingerID = -1;
                        leftTouchScreenStartPosition = Vector2.zero;
                        currentCameraAcceleration = 0.0f; // Reset the camera accelaration when the left/movement touch is ended
                    }
                    else if (rightSideFingerID == touch.fingerId)
                    {
                        rightSideFingerID = -1;
                        rightTouchScreenStartPosition = Vector2.zero;
                    }
                }
            }
        }
    }

    // Handles the zoom in/out (returns true if the player is zooming in/out)
    bool ApplyPhonePinch()
    {
        // Check if the player is trying to pinch the screen with 2 fingers (zoom in/out)
        if (Input.touches.Length == 2)
        {
            // Check which direction the player is trying to pinch their phone at
            foreach (Touch touch in Input.touches) 
            {
                // Make sure that all the finger touches match and exist (-1 finger ID = doesn't exist)
                if (leftSideFingerID == touch.fingerId && rightSideFingerID >= 0)
                {
                    Vector3 direction = mainCamera.transform.forward;
                    Vector3 newCameraPosition = Vector3.zero;

                    // Zoom out
                    if (touch.position.x > leftTouchScreenStartPosition.x && Input.GetTouch(rightSideFingerID).position.x < rightTouchScreenStartPosition.x)
                    {
                        newCameraPosition = mainCamera.transform.position + (direction * zoomAmountPerScroll);

                    }
                    // Zoom in
                    else if(touch.position.x < leftTouchScreenStartPosition.x && Input.GetTouch(rightSideFingerID).position.x > rightTouchScreenStartPosition.x)
                    {
                        newCameraPosition = mainCamera.transform.position + (-direction * zoomAmountPerScroll);

                    }

                    // Making sure that we are keeping the distance from the camera to the offset limited within the min/max zoom distances
                    float yDistance = Mathf.Abs(newCameraPosition.y - pointingCoords.y);
                    if (yDistance > minZoomDistance || yDistance < maxZoomDistance)
                    {
                        newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, minZoomDistance, maxZoomDistance);

                        mainCamera.transform.position = newCameraPosition;
                        return true;

                    }
                }
                else if(rightSideFingerID == touch.fingerId && leftSideFingerID >= 0)
                {

                    Vector3 direction = mainCamera.transform.forward;
                    Vector3 newCameraPosition = Vector3.zero;

                    // Zoom out
                    if (touch.position.x > rightTouchScreenStartPosition.x && Input.GetTouch(leftSideFingerID).position.x < leftTouchScreenStartPosition.x)
                    {
                        newCameraPosition = mainCamera.transform.position + (direction * zoomAmountPerScroll);

                    }
                    // Zoom in
                    else if (touch.position.x < rightTouchScreenStartPosition.x && Input.GetTouch(leftSideFingerID).position.x > leftTouchScreenStartPosition.x)
                    {
                        newCameraPosition = mainCamera.transform.position + (-direction * zoomAmountPerScroll);

                    }


                    // Making sure that we are keeping the distance from the camera to the offset limited within the min/max zoom distances
                    float yDistance = Mathf.Abs(newCameraPosition.y - pointingCoords.y);
                    if (yDistance > minZoomDistance || yDistance < maxZoomDistance)
                    {
                        newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, minZoomDistance, maxZoomDistance);

                        mainCamera.transform.position = newCameraPosition;
                        return true;

                    }
                }
            }
        }
        return false;
    }

    void ApplyLeftSidePhoneTouch(Touch touch)
    {
        // Check if the player is touching the left half side of the screen
        if (touch.position.x < Screen.width / 2.0f)
        {
            // Save the current finger the player is using to operate their device
            if (leftSideFingerID != touch.fingerId)
            {
                leftSideFingerID = touch.fingerId;
                leftTouchScreenStartPosition = touch.position;
            }
        }
    }

    void ApplyRightSidePhoneTouch(Touch touch)
    {
        // Check if the player is touching the right half side of the screen
        if (touch.position.x >= Screen.width / 2.0f)
        {
            // Save the current finger the player is using to operate their device
            if (rightSideFingerID != touch.fingerId)
            {
                rightSideFingerID = touch.fingerId;
                rightTouchScreenStartPosition = touch.position;
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

            if (horizontalInput != 0 || verticalInput != 0)
            {
                mainCamera.transform.position += moveDir * cameraMovingSpeed * currentCameraAcceleration * Time.deltaTime;
            }
            //print("Are coords in barrier: " + AreCoordsInBox(mainCamera.transform.position, cameraFreeroamBoxCollider));
            //print("Camera bounding box max bounds coords: " + cameraFreeroamBoxCollider.bounds.max);
            //print("Camera bounding box min bounds coords: " + cameraFreeroamBoxCollider.bounds.min);
        }
    }

    void HandleCameraZoom()
    {
		// Check if the mouse IS NOT over any UI element object
		if (!(bool)(EventSystem.current?.IsPointerOverGameObject()))
        {
			Vector2 mouseScrollDelta = Input.mouseScrollDelta; // Scroll wheel value
			if (mouseScrollDelta.y != 0)
			{
				//Vector3 direction = (offsetCoords - mainCamera.transform.position).normalized;
				Vector3 direction = mainCamera.transform.forward;
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
				float yDistance = Mathf.Abs(newCameraPosition.y - pointingCoords.y);
				if (yDistance < minZoomDistance || yDistance > maxZoomDistance) return;

				// Try to clamp the y-value of the new position in case it exceeds the min/max zoom distances
				newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, minZoomDistance, maxZoomDistance);

				mainCamera.transform.position = newCameraPosition;
			}
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

    // Stop the camera's movement instantly
    void StopCamera()
    {
        currentCameraAcceleration = 0.0f;
    }
}

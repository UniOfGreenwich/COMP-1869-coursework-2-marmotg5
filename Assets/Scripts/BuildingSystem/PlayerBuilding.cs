using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BuildingState
{
    NONE,
    CHOOSING_OBJECT,
    PLACING_OBJECT
}

public class PlayerBuilding : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject playerBuildingUIPrefab;

	[Header("Keyboard Controls")]
    [SerializeField]
    KeyCode buildingStateKey = KeyCode.Y;

    GameObject playerBuildingUIGameObject = null;

    BuildingState buildingState;
    Camera mainCamera;

	GridObjectData selectedObjectData = null;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        buildingState = BuildingState.NONE;
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBuildingStates();
        HandleBuilding();
	}

    void HandleBuildingStates()
    {
        if (Input.GetKeyDown(buildingStateKey))
        {
            // Start building
            if (buildingState == BuildingState.NONE)
            {
                SetBuildingState(BuildingState.CHOOSING_OBJECT);
                OpenBuildingMenu();
            }

            // Exit building
            else if(buildingState == BuildingState.CHOOSING_OBJECT || buildingState ==  BuildingState.PLACING_OBJECT)
            {
				SetBuildingState(BuildingState.NONE);
                CloseBuildingMenu();

			}
        }
    }

    void HandleBuilding()
    {
		//if (buildingState == BuildingState.CHOOSING_OBJECT || buildingState == BuildingState.PLACING_OBJECT)
		if (buildingState == BuildingState.CHOOSING_OBJECT)
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit raycastHit))
			{
                GridCell gridCell = GridSystem.instance.GetGridCellFromCoords(raycastHit.point);
                
                // Make sure player is clicking with the grid system and a grid cell has been found
                if (gridCell != null)
                {
					if (Input.GetMouseButtonDown((int)MouseButton.Left))
					{
                        // Check if the mouse IS NOT over any UI element object
                        if (!(bool)(EventSystem.current?.IsPointerOverGameObject()))
                        {
                            GridSystem.instance.SpawnGridObject(gridCell);
                        }

					}
				}
            }

        }
	}

    void OpenBuildingMenu()
    {
        if (GameManager.gridSystem != null)
        {
            // Activate the grid system
			GameManager.gridSystem.SetGridSystemRendering(true);
            
            // Try to create the player building UI
            if (playerBuildingUIPrefab != null && GameManager.UIManager != null)
            {
                playerBuildingUIGameObject = Instantiate(playerBuildingUIPrefab, GameManager.UIManager.transform);
				PlayerBuildingUI buildingUI = playerBuildingUIGameObject.GetComponent<PlayerBuildingUI>();
                if (buildingUI != null)
                {
                    buildingUI.PopulateBuildingMenu(); // Populate building menu with items from the player's inventory (if any)
                }
			}
		}

	}

    void CloseBuildingMenu()
    {
		if (GameManager.gridSystem != null)
		{
            // Close the grid system UI
			GameManager.gridSystem.SetGridSystemRendering(false);

            // Remove the building UI
            if (playerBuildingUIGameObject != null)
            {
                Destroy(playerBuildingUIGameObject);
                playerBuildingUIGameObject = null;

			}

			selectedObjectData = null;
		}

	}


	public void SetBuildingState(BuildingState state)
    {
        buildingState = state;
    }

    void SwitchBuildingObject()
    {

    }

    BuildingState GetBuildingState() {return buildingState;}
}

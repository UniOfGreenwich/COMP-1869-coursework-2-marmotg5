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
    BuildingState buildingState;

    Camera mainCamera;
    [Header("Grid Building Objects Data")]

    GridObjectData selectedObjectData = null;

    [Header("Keyboard Controls")]
    [SerializeField]
    KeyCode buildingStateKey = KeyCode.Y;

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
            print("Changed Building State To: " + buildingState.ToString());
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
						//else
						//{
						//	// The click was not on a UI element
						//	Debug.Log("The pointer is not over a UI object.");
						//}

						//GridSystem.instance.SpawnGridObject(gridCell);

      //                  GridObjectData gridObjectData = allowedGridObjects[0];
      //                  if (gridObjectData != null)
      //                  {
      //                      Instantiate(gridObjectData.objectPrefab, gridCell.GetCenteredPosition());

						//	//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						//	//cube.transform.position = gridCell.GetCenteredPosition();
						//}


					}
				}
            }

        }
	}

    void OpenBuildingMenu()
    {
        if (GameManager.gridSystem != null)
        {
			GameManager.gridSystem.SetGridSystemRendering(true);
		}

	}

    void CloseBuildingMenu()
    {
		if (GameManager.gridSystem != null)
		{
			GameManager.gridSystem.SetGridSystemRendering(false);
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

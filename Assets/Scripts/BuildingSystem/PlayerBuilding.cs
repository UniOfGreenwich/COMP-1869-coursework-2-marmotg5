using Unity.VisualScripting;
using UnityEngine;

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
    //GridSystem gridSystem;

    [Header("Keyboard Controls")]
    [SerializeField]
    KeyCode buildingStateKey = KeyCode.Y;

	[Header("Mouse Controls")]
	[SerializeField]
    MouseButton cancelPlacementButton = MouseButton.Right;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        buildingState = BuildingState.NONE;
        mainCamera = GetComponent<Camera>();
        //gridSystem = GridSystem.instance;
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
                DisplayGridSystem();
            }

            // Exit building
            else if(buildingState == BuildingState.CHOOSING_OBJECT || buildingState ==  BuildingState.PLACING_OBJECT)
            {
				SetBuildingState(BuildingState.NONE);
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
			    if (gridCell != null)
                {
					print(raycastHit.point);
				}
			}
			//if (Input.GetMouseButtonDown((int)MouseButton.Left))
			//{
			//	Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			//	if (Physics.Raycast(ray, out RaycastHit raycastHit))
			//	{
			//		print(raycastHit.point);
			//	}
			//}
		}
	}

	void DisplayGridSystem()
    {
		// TODO
		// TODO
		// TODO
		// TODO
		// TODO
		// TODO
	}

	public void SetBuildingState(BuildingState state)
    {
        buildingState = state;
    }

    BuildingState GetBuildingState() {return buildingState;}
}

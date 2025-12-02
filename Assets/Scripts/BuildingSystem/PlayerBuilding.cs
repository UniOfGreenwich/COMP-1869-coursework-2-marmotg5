using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BuildingState
{
    NONE,
    CHOOSING_OBJECT
}

public class PlayerBuilding : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject playerBuildingUIPrefab;

	[Header("Keyboard Controls")]
    [SerializeField]
    KeyCode buildingStateKey = KeyCode.Y;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip uiClickSFX;

    GameObject playerBuildingUIGameObject = null;

    BuildingState buildingState;
    Camera mainCamera;

    bool isUIToggled = false;

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
            else if(buildingState == BuildingState.CHOOSING_OBJECT)
            {
				SetBuildingState(BuildingState.NONE);
                CloseBuildingMenu();

			}
        }
    }

    // Turn on/off the building UI for the grid system (Called from a UI button)
    public void ToggleBuildingMenu()
    {
        // Turn on
        if (!isUIToggled)
        {
            SetBuildingState(BuildingState.CHOOSING_OBJECT);
            OpenBuildingMenu();
        }
        // Turn off
        else
        {
            SetBuildingState(BuildingState.NONE);
            CloseBuildingMenu();
        }
    }

    void OpenBuildingMenu()
    {
        if (isUIToggled) return; // Menu is already toggled on and working

        if (audioSource != null && uiClickSFX != null)
        {
            audioSource.PlayOneShot(uiClickSFX);
        }

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
        isUIToggled = true;
	}

    void CloseBuildingMenu()
    {
        if (!isUIToggled) return; // Menu is already turned off

        if (audioSource != null && uiClickSFX != null)
        {
            audioSource.PlayOneShot(uiClickSFX);
        }

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
		}
        isUIToggled = false;
	}


	public void SetBuildingState(BuildingState state)
    {
        buildingState = state;
    }

    BuildingState GetBuildingState() {return buildingState;}
}

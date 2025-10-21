using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	// Plant UI
	GameObject currentPlantUIGameObject = null; // The current plant the player has clicked on to show it's UI
	PlantUI currentPlantUI = null;
	Vector3 plantUIOffset = new Vector3(0.0f, -100.0f, 0.0f);
	IEnumerator trackCurrentPlantUICoroutine = null;

	// Plant object
	PlantObject currentPlantObject = null;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
	}

    public void CreatePlantUI(PlantObject plantObject, GameObject plantUIPrefab)
    {
		Camera mainCamera = GameManager.mainCamera;

		// Check if there is already a plant object with UI rendered on it
		if (currentPlantObject != null)
		{
			// Check if the player is trying to create plant UI in the same location another plant UI already exists
            if (plantObject.GetObjectGridCell().GetCellIndex() == currentPlantObject.GetObjectGridCell().GetCellIndex()) 
			{
                RemovePlantUI();
                return; // Ignore the rest of the function as the player wanted to hide the UI of the current selected plant
			}
			// Player has clicked on a different plant, so we delete the old existing UI and continue with the function
			else
			{
				RemovePlantUI();
			}
        }

        // Create a new plant UI for the UI manager to keep track of and follow the camera
        if (mainCamera != null && plantObject != null)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(plantObject.transform.position); // Convert the plant's 3D position into a 2D screen position

            // Set the UI manager script to store data of the current plant UI the player is trying to use/look at
			currentPlantUIGameObject = Instantiate(plantUIPrefab, screenPosition + plantUIOffset, Quaternion.identity, transform);
			currentPlantUI = currentPlantUIGameObject.GetComponent<PlantUI>();
            currentPlantObject = plantObject;

            // Start a coroutine that tracks the current plant object the player has clicked on
            trackCurrentPlantUICoroutine = TrackCurrentPlantUI(plantObject, mainCamera);
            StartCoroutine(trackCurrentPlantUICoroutine);
		}
	}

	IEnumerator TrackCurrentPlantUI(PlantObject plantObject, Camera renderCamera)
	{
		// While the current plant we are tracking exists
		while (plantObject != null && plantObject == currentPlantObject)
		{
			Vector3 screenPosition = renderCamera.WorldToScreenPoint(plantObject.transform.position);
			currentPlantUIGameObject.transform.position = screenPosition + plantUIOffset;

			yield return new WaitForSeconds(Time.deltaTime);
		}
		// Cleanup
		Destroy(currentPlantUIGameObject);
		StopCoroutine(trackCurrentPlantUICoroutine);
	}

	public void RemovePlantUI()
	{
		// Stop the coroutine that was tracking the previous plant, if it was running.
		if (trackCurrentPlantUICoroutine != null)
		{
			StopCoroutine(trackCurrentPlantUICoroutine);
			trackCurrentPlantUICoroutine = null;
		}

		// Destroy the previous UI GameObject if it exists.
		if (currentPlantUIGameObject != null)
		{
			Destroy(currentPlantUIGameObject);

			// Clear all references associated with the old plant/UI
			currentPlantUIGameObject = null;
			currentPlantUI = null;
			currentPlantObject = null;
		}
	}
}

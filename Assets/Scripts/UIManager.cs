using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	// Plant UI
	GameObject currentPlantUIGameObject = null; // The current plant the player has clicked on to show it's UI
	PlantUI currentPlantUI = null;
	Vector3 plantUIOffset = new Vector3(0.0f, -100.0f, 0.0f);
	IEnumerator trackCurrentPlantUICoroutine = null;

	// Plant
	PlantObject currentPlantObject = null;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        //MakeUIFollowPlant();

	}

    public void CreatePlantUI(PlantObject plantObject, GameObject plantUIPrefab)
    {
        currentPlantObject = plantObject;
		Camera mainCamera = GameManager.mainCamera;

		// Check in case the player has clicked on a different plant before
		if (currentPlantUIGameObject != null)
        {
            // Destroy the previous plant's UI
            currentPlantUI = currentPlantUIGameObject.GetComponent<PlantUI>();
            if (currentPlantUI != null) currentPlantUI.DestroyUI();

			currentPlantUIGameObject = null; // The UI GAME OBJECT for it
			currentPlantUI = null; // The UI script for it
            currentPlantObject = null; // The actual plant in 3D

		}

        // Create a new plant UI for the UI manager to keep track of and follow the camera
        if (mainCamera != null)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(plantObject.transform.position); // Convert the plant's 3D position into a 2D screen position

            // Set the UI manager script to store data of the current plant UI the player is trying to use/look at
			currentPlantUIGameObject = Instantiate(plantUIPrefab, screenPosition + plantUIOffset, Quaternion.identity, transform);
			currentPlantUI = (currentPlantUIGameObject.GetComponent<PlantUI>() != null) ? currentPlantUIGameObject.GetComponent<PlantUI>() : null;

            // Start a coroutine that tracks the current plant object the player has clicked on
            trackCurrentPlantUICoroutine = TrackCurrentPlantUI(plantObject, mainCamera);
            StartCoroutine(trackCurrentPlantUICoroutine);
		}
    }

    IEnumerator TrackCurrentPlantUI(PlantObject plantObject, Camera renderCamera)
    {
        while (plantObject != null || plantObject.gameObject == currentPlantObject.gameObject)
        {
			Vector3 screenPosition = renderCamera.WorldToScreenPoint(plantObject.transform.position);
            currentPlantUIGameObject.transform.position = screenPosition + plantUIOffset;

			yield return new WaitForSeconds(Time.deltaTime);
		}
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT
		// MAKE IT SO THE OLD UI GETS DELETED AND REPLACED WITH A NEW UI WHEN PLAYER CLICKS ON DIFF PLANT

		Destroy(currentPlantUIGameObject);

		yield return null;
    }

    //void MakeUIFollowPlant()
    //{
    //    if (currentPlantUI != null && currentPlantUIGameObject != null)
    //    {
    //        if (GameManager.mainCamera != null)
    //        {
    //            Camera mainCamera = GameManager.mainCamera;
    //            Vector3 screenPosition = mainCamera.WorldToScreenPoint(currentPlantUIGameObject.transform.position);
    //            Vector3 newUIPosition = screenPosition + plantUIOffset;
    //        }
    //    }
    //}
}

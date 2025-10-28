using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ShopSystem : MonoBehaviour
{
    [Header("The Shop UI Menu")]
    [SerializeField] GameObject shopUIPrefab;

    [Header("Shop Items"), Tooltip("The items that will be shown for purchase in the shop")]
    [SerializeField] GridPlantData[] shopItemsArray; // Placeholder array to just tell the shop what items it will have inside

    GridPlantData[] organizedItemArray; // The actual organized array with all the items from "shopItemsArray"

    // Shop UI
    GameObject currentShopUIGameObject = null;

    bool isShopOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		organizedItemArray = GetOrganizedShopByPlantLevel();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the shop is clicked on by the player
	void OnMouseDown()
	{
		if (!(bool)(EventSystem.current?.IsPointerOverGameObject()))
		{
			ToggleShopUI();
		}
	}

	void ToggleShopUI()
	{
		// Make sure we have a UI manager handling the shop UI and items to populate the shop UI with
		if (organizedItemArray != null && organizedItemArray.Length > 0)
		{
            CreateShopUI();
		}
	}

    void CreateShopUI()
    {
        // If we don't have a shop created
        if (currentShopUIGameObject == null && GameManager.UIManager != null)
        {
            // Create a shop
            currentShopUIGameObject = Instantiate(shopUIPrefab, GameManager.UIManager.transform);
            ShopUI shopUI = currentShopUIGameObject.GetComponent<ShopUI>();
            if (shopUI != null)
            {
                shopUI.PopulateShop(organizedItemArray); // Populate the shop list with items
            }
        }
        // We already have a shop created, so delete it
        else
        {
            RemoveShopUI();
        }
    }

    public void RemoveShopUI()
    {
        if (currentShopUIGameObject != null)
        {
            Destroy(currentShopUIGameObject);
            currentShopUIGameObject = null;
        }
    }

    // Function that will organize an array which will start with the smallest level of plants at the beginning and put the highest levels at the end
    GridPlantData[] GetOrganizedShopByPlantLevel()
    {
        if (shopItemsArray.Length > 0)
        {
			GridPlantData[] organizedPlantArray = new GridPlantData[shopItemsArray.Length];

            int minPlantLevel = GetMinPlantLevelFromArray(shopItemsArray); // Get the smallest level of a plant within the array
			int maxPlantLevel = GetMaxPlantLevelFromArray(shopItemsArray); // Get the highest

			// For each level (range - mininum to maximum level out of the array)
            for (int currentShopLevel = minPlantLevel; currentShopLevel <= maxPlantLevel; currentShopLevel++)
            {
				// Check each plant's level
                for (int plantIndex = 0; plantIndex < shopItemsArray.Length; plantIndex++)
                {
					// Check if the current plant data we are looping through has the same level that we are going through
                    GridPlantData plantData = shopItemsArray[plantIndex];
                    if (plantData.objectRequiredLevel == currentShopLevel)
                    {
						organizedPlantArray[currentShopLevel] = plantData;
                    }
                }

            }

            return organizedPlantArray;
		}

        return null;
	}

	// Simple function that returns the lowest level value out of an array
    int GetMinPlantLevelFromArray(GridPlantData[] plantDataArray)
    {
        int minLevel = 0;

        if (plantDataArray.Length > 0)
        {
			foreach (GridPlantData plantData in plantDataArray)
			{
				if (plantData.objectRequiredLevel < minLevel)
					minLevel = plantData.objectRequiredLevel;
			}
		}

        return minLevel;
    }

	// Same thing as above, but returns the max
	int GetMaxPlantLevelFromArray(GridPlantData[] plantDataArray)
	{
        int maxLevel = 0;

		if (plantDataArray.Length > 0)
		{
			foreach (GridPlantData plantData in plantDataArray)
			{
				if (plantData.objectRequiredLevel > maxLevel)
					maxLevel = plantData.objectRequiredLevel;
			}
		}

		return maxLevel;
	}

}

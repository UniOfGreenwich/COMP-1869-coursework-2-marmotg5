using UnityEngine;
using UnityEngine.Rendering;

public class ShopSystem : MonoBehaviour
{
    [Header("The Shop UI Menu")]
    [SerializeField] GameObject shopUIPrefab;

    [Header("Shop Items"), Tooltip("The items that will be shown for purchase in the shop")]
    [SerializeField] GridPlantData[] shopItemsArray; // Placeholder array to just tell the shop what items it will have inside

    GridPlantData[] organizedItemArray; // The actual organized array with all the items from "shopItemsArray"

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
		ToggleShopUI();
	}

	void ToggleShopUI()
	{
		// Make sure we have a UI manager handling the shop UI and items to populate the shop UI with
		if (GameManager.UIManager != null && organizedItemArray != null && organizedItemArray.Length > 0)
		{
			isShopOpen = !isShopOpen;

			if (isShopOpen) // Open
			{
				GameManager.UIManager.CreateShopUI(organizedItemArray, shopUIPrefab);
			}
			else // Close
			{
				GameManager.UIManager.RemoveShopUI();
			}
		}
	}

	GridPlantData[] GetOrganizedShopByPlantLevel()
    {
        if (shopItemsArray.Length > 0)
        {
			GridPlantData[] organizedPlantArray = new GridPlantData[shopItemsArray.Length];

            int minPlantLevel = GetMinPlantLevelFromArray(shopItemsArray);
			int maxPlantLevel = GetMaxPlantLevelFromArray(shopItemsArray);

            for (int currentShopLevel = minPlantLevel; currentShopLevel <= maxPlantLevel; currentShopLevel++)
            {
                for (int plantIndex = 0; plantIndex < shopItemsArray.Length; plantIndex++)
                {
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

    public void BuyCrop(GridPlantData plantData)
    {
        Player player = GameManager.player;
        if (player != null)
        {
            bool hasEnoughLevel = (player.GetLevel() >= plantData.objectRequiredLevel);
			bool hasEnoughCash = (player.GetCash() - plantData.objectCost) >= 0;

			if (hasEnoughLevel && hasEnoughCash)
            {
                player.RemoveCash(plantData.objectCost);
			}
		}
    }
}

using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject shopUI;

    [SerializeField] GridPlantData[] plantShopArray; // The items the shop will sell

    bool isShopOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the shop is clicked on by the player
	void OnMouseDown()
	{
        print("Shop was opened");	
	}

    void ConstructShop()
    {
        GridPlantData[] organizedPlantArray = GetOrganizedShopByPlantLevel();
        if (organizedPlantArray != null)
        {

        }
    }

    GridPlantData[] GetOrganizedShopByPlantLevel()
    {
        if (plantShopArray.Length > 0)
        {
			GridPlantData[] organizedArray = new GridPlantData[plantShopArray.Length];
            for (int i = 0; i < organizedArray.Length; i++)
            {
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
				// ORGANIZE BYHHH LEVEL AND RETURN NEW ARRAY
			}

		}

        return null;
	}

    // Toggle the UI of the shop
    void ToggleShop()
    {
        isShopOpen = !isShopOpen;

        if (isShopOpen) // Open
        {

        }
        else // Close
        {

        }
    }

    public void BuyCrop(GridPlantData plantData)
    {
        Player player = GameManager.player;
        if (player != null)
        {
            bool hasEnoughLevel = (player.GetLevel() >= plantData.objectRequiredLevel);
            if (hasEnoughLevel)
            {
				int plantCost = plantData.objectCost;
			}
		}
    }
}

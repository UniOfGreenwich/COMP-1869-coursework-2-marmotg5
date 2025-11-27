using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] GameObject shopItemUIPrefab;

    [Header("Shop List Parent"), Tooltip("Where the shop items will be placed within")]
    [SerializeField] Transform shopItemList;

	[Header("UI Buttons")]
    [SerializeField] Button shopUICloseButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Populates/re-popualtes the shop with items we want the UI to display
    public void PopulateShop(List<GridPlantData> itemsList)
    {
        foreach (GridPlantData item in itemsList)
        {
            // Create the shop item UI and add it to the UI's list
            GameObject shopItemUIGameObject = Instantiate(shopItemUIPrefab, shopItemList);

            // Update the template data with the actual plant's info
            ShopItemUI shopItemUI = shopItemUIGameObject.GetComponent<ShopItemUI>();
            if (shopItemUI != null)
            {
                shopItemUI.UpdateShopItemData(item);
            }
        }
    }

    // Gets called from a button event set in the inspector of the prefab
    public void RemoveUI()
    {
        if (GameManager.UIManager != null)
        {
            Destroy(gameObject);
        }
	}
}

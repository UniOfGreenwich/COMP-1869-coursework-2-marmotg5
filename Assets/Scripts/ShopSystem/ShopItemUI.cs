using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI Prefixes")]
    [SerializeField] string levelPrefix = "Level: ";
	[SerializeField] string costPrefix = "Cost: $";

	[Header("UI Text Elements")]
	[SerializeField] TextMeshProUGUI cropNameText;
	[SerializeField] TextMeshProUGUI cropLevelText;
	[SerializeField] TextMeshProUGUI cropCostText;

	[Header("UI Image Elements")]
    [SerializeField] Image cropImage;

	[Header("UI Button Elements")]
	[SerializeField] Button buyButton;

    GridPlantData shopItemData = null;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateShopItemData(GridPlantData plantData)
    {
        if (plantData == null) return;
        shopItemData = plantData;

        // Update the template UI elements with the necessary information
        cropNameText.text = shopItemData.objectName;
        cropLevelText.text = levelPrefix + shopItemData.objectRequiredLevel.ToString();
        cropCostText.text = costPrefix + shopItemData.objectCost.ToString();

        cropImage.sprite = shopItemData.objectSprite;

        buyButton.onClick.AddListener(BuyItem);
    }

	public void BuyItem()
	{
		if (shopItemData == null) return; // Won't be null since we save a reference to it above but just in case

		Player player = GameManager.player;
		if (player != null)
		{
			bool hasEnoughLevel = (player.GetLevel() >= shopItemData.objectRequiredLevel);
			bool hasEnoughCash = (player.GetCash() - shopItemData.objectCost) >= 0;

			if (hasEnoughLevel && hasEnoughCash)
			{
				player.RemoveCash(shopItemData.objectCost);


				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
				// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH// LINK TO INVENTORY AND ADD THE ITEM AFTER TAKING AWAY PLAYER CASH
			}
		}
	}
}

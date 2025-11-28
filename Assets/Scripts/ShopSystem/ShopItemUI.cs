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
	Color defaultBuyButtonColour;

    [Header("Audio stuff")]
    [SerializeField] private AudioClip buySFX;
    private AudioSource audioSource;

    GridPlantData shopItemData = null;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
    }

    void Awake()
    {
        // Stores the default buy button colour (displaying it later to indicate the player we can buy the item)
        if (buyButton != null)
        {
            defaultBuyButtonColour = buyButton.image.color;
            
        audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateShopItemData(GridPlantData plantData)
    {
        if (plantData == null) return;
        shopItemData = plantData;

        Player player = GameManager.player;

        // Update the template UI elements with the necessary information
        cropNameText.text = shopItemData.objectName;
        cropLevelText.text = levelPrefix + shopItemData.objectRequiredLevel.ToString();
        cropCostText.text = costPrefix + shopItemData.objectCost.ToString();

        cropImage.sprite = shopItemData.objectSprite;

        // Set the colour of the UI button (if the player has enough level and cash)
		if (buyButton != null && player != null)
		{
            Image buyButtonImage = buyButton.image;

			if (player.GetLevel() >= plantData.objectRequiredLevel && player.GetCash() >= plantData.objectCost)
			{
				// Player can buy the seed/crop
                buyButtonImage.color = defaultBuyButtonColour;
            }
            else
			{
				// Not enough level
                buyButtonImage.color = Color.red;
            }

            buyButton.onClick.AddListener(BuyItem);
        }
    }

    void PlayBuySFX()
    {
        if (audioSource != null && buySFX != null)
        {
            audioSource.PlayOneShot(buySFX);
        }
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
				player.GetInventory().AddItem(shopItemData);

                PlayBuySFX();
			}
		}
	}
}

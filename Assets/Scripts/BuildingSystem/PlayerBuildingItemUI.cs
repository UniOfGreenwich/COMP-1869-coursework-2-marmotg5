using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingItemUI : MonoBehaviour
{
    [Header("UI Prefixes")]
    [SerializeField] string quantityPrefix = "Quantity: ";

    [Header("UI Text")]
	[SerializeField] TextMeshProUGUI itemNameText;
	[SerializeField] TextMeshProUGUI itemQuantityText;

	[Header("UI Image")]
	[SerializeField] Image itemImage;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInventoryItemUI(InventoryItem inventoryItem)
    {
        if (inventoryItem == null) return;

        itemNameText.text = inventoryItem.itemData.objectName;
        itemQuantityText.text = quantityPrefix + inventoryItem.quantity.ToString();
        itemImage.sprite = inventoryItem.itemData.objectSprite;
    }
}

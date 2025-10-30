using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerBuildingUI : MonoBehaviour
{
	[Header("Inventory Item UI Prefab")]
	[SerializeField] GameObject inventoryItemPrefab;

	[Header("Inventory List Parent"), Tooltip("Where the inventory items will be placed within")]
	[SerializeField] Transform inventoryItemList;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PopulateBuildingMenu()
	{
		Player player = GameManager.player != null ? GameManager.player : GetComponent<Player>();
		if (player != null)
		{
			InventorySystem playerInventory = player.GetInventory();
			List<InventoryItem> inventoryItems = playerInventory.GetItems();
			
			// Loop through all the inventory items and place them in the UI menu
			if (inventoryItems.Count > 0)
			{
				foreach (InventoryItem item in inventoryItems)
				{
					GameObject inventoryItemGameObject = Instantiate(inventoryItemPrefab, inventoryItemList);
					PlayerBuildingItemUI inventoryItemUI = inventoryItemGameObject.GetComponent<PlayerBuildingItemUI>();
					if (inventoryItemUI != null)
					{
						inventoryItemUI.UpdateInventoryItemUI(item);
					}
				}
			}
		}
	}
}

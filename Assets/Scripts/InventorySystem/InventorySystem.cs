using UnityEngine;
using System.Collections.Generic;

public class InventorySystem
{
    [SerializeField] List<InventoryItem> items = new List<InventoryItem>();     // Holds all of the items

    public void AddItem(GridObjectData plantItemData)  // Add an item to the inventory
    {
        // Try and check if we already have an item in the list with the same data
        InventoryItem similarItem = FindSameItem(plantItemData);
        if (similarItem != null)
        {
            similarItem.IncreaseQuantity();
        }
        // We don't have a similar object in the inventory, so we add a new one
        else
        {
            items.Add(new InventoryItem(plantItemData));
        }
    }

	public void AddItem(GridObjectData plantItemData, int quantity) // Add an item with a set quantity
	{
		// Try and check if we already have an item in the list with the same data
		InventoryItem similarItem = FindSameItem(plantItemData);
		if (similarItem != null)
		{
			similarItem.IncreaseQuantity();
		}
		// We don't have a similar object in the inventory, so we add a new one
		else
		{
            // Make the new inventory item
            InventoryItem newItem = new InventoryItem(plantItemData);
            newItem.quantity = quantity;

			items.Add(newItem);
		}
	}

	public void RemoveItem(InventoryItem item) // Remove an item from the inventory
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            //Debug.Log(item.itemName + " has been removed from your inventory");
        }
        else
        {
            Debug.Log("This item is not in your inventory");
        }
   
    }

    public void UseItem(InventoryItem itemToUse)
    {
        itemToUse.DecreaseQuantity();

        // Item is finished and has no quantity left
        if (itemToUse.quantity <= 0)
        {
            RemoveItem(itemToUse);
        }
    }

    // This function will go through the current inventory and will check if we already have an item with that's using the same data
    InventoryItem FindSameItem(GridObjectData itemDataToCheck)
    {
        foreach (InventoryItem loopedItem in items)
        {
            if (itemDataToCheck == loopedItem.itemData)
                return loopedItem;
        }

        return null;
    }

    public void ShowInventory()
    {
        if (items.Count == 0)
        {
            Debug.Log("There is nothing in your inventory");
        }
        foreach (InventoryItem item in items)
        {
            Debug.Log("- " + item.itemData.objectName + ": "+item.quantity+"x amount");
        }
    }

    public List<InventoryItem> GetItems() {  return items; }
}

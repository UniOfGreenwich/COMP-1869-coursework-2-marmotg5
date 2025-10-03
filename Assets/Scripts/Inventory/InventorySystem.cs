using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;

public class InventorySystem : MonoBehaviour
{
    public List<Item> items = new List<Item>();     // Holds all of the items
    
    public void AddItem(Item item)  // Add an item to the inventory
    {
        items.Add(item);
        Debug.Log(item.itemName + " is now in your inventory");
    }

    public void RemoveItem(Item item)   // Remove an item from the inventory
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log(item.itemName + " has been removed from your inventory");
        }
        else
        {
            Debug.Log("This item is not in your inventory");
        }
   
    }

    public void ShowInventory() // Show items in inventory
    {
        if (items.Count == 0)
        {
            Debug.Log("There is nothing in your inventory");
        }
        foreach (Item item in items)
        {
            Debug.Log("- "+ item.itemName);
        }
    }
}

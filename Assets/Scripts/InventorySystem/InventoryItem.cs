using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public GridPlantData itemData;
    public int quantity;

    public InventoryItem(GridPlantData plantData)
    {
        itemData = plantData;
        quantity = 1; // Create a new item with a default quantity of 1
    }
    
    public void IncreaseQuantity()
    {
        quantity++;
    }
}

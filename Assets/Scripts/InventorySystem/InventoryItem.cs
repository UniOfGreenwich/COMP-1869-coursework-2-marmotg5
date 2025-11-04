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

    public void DecreaseQuantity()
    {
        quantity--;
        if (quantity < 0)
        {
            quantity = 0;
        }
    }
}

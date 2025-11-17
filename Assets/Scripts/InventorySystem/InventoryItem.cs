using UnityEngine;

public class InventoryItem
{
    public GridObjectData itemData;
    public int quantity;

    public InventoryItem(GridObjectData plantData)
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

using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public GridPlantData itemData;
    public int quantity;

    public InventoryItem(GridPlantData plantData)
    {
        itemData = plantData;
        quantity = 0;
    }
    
    public void IncreaseQuantity()
    {
        quantity++;
    }
}

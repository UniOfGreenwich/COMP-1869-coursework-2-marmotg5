using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
	public int playerCash;
	public int playerLevel;

	public InventorySystem inventorySystem; // The player's inventory with all their items inside
	public List<GridCell> occupiedGridCells; // All the grid cells that are occupied with plants
}

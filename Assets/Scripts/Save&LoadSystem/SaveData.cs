using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
	[System.Serializable]
	public class GridCellData
	{
		public int cellZ; // Z location on the grid system
		public int cellX; // X location on the grid system 
		public string prefabName;

		public GridCellData((int z, int x) cellIndex, string prefabName)
		{
			this.cellZ = cellIndex.z;
			this.cellX = cellIndex.x;
			this.prefabName = prefabName;
		}

		// FIX THE ERROR WHERE THE CLEAN LIST IN GameManager.cs IS REMOVING
		// RANDOM CELLS AND LEAVES ONLY 1, MAKE IT SO IT ONLY LEAVES THE HOST/PARENT CELL WHERE
		// THE PLANT WAS ACTUALLY PLACED ON IT

		// FIX THE ERROR WHERE THE CLEAN LIST IN GameManager.cs IS REMOVING
		// RANDOM CELLS AND LEAVES ONLY 1, MAKE IT SO IT ONLY LEAVES THE HOST/PARENT CELL WHERE
		// THE PLANT WAS ACTUALLY PLACED ON IT

		// FIX THE ERROR WHERE THE CLEAN LIST IN GameManager.cs IS REMOVING
		// RANDOM CELLS AND LEAVES ONLY 1, MAKE IT SO IT ONLY LEAVES THE HOST/PARENT CELL WHERE
		// THE PLANT WAS ACTUALLY PLACED ON IT

		// FIX THE ERROR WHERE THE CLEAN LIST IN GameManager.cs IS REMOVING
		// RANDOM CELLS AND LEAVES ONLY 1, MAKE IT SO IT ONLY LEAVES THE HOST/PARENT CELL WHERE
		// THE PLANT WAS ACTUALLY PLACED ON IT

		// FIX THE ERROR WHERE THE CLEAN LIST IN GameManager.cs IS REMOVING
		// RANDOM CELLS AND LEAVES ONLY 1, MAKE IT SO IT ONLY LEAVES THE HOST/PARENT CELL WHERE
		// THE PLANT WAS ACTUALLY PLACED ON IT
	}

	[System.Serializable]
	public class InventoryData
	{
		public string prefabName; // The prefab object that should be spawned in the world
		public int quantity; // The amount of the object the player has

		public InventoryData(InventoryItem inventoryItem)
		{
			prefabName = inventoryItem.itemData.objectPrefab.name;
			quantity = inventoryItem.quantity;
		}
	}

	public int playerCash;
	public int playerLevel;

	public List<InventoryData> inventoryData = new List<InventoryData>(); // The player's inventory with all their items inside
	public List<GridCellData> occupiedGridCells = new List<GridCellData>(); // All the grid cells that are occupied with plants
}

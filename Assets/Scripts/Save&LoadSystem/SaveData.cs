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
		
		public string prefabName; // The prefab name of the grid object

		// Data about the plant we could be storing
		public int plantHealth;
		public float plantWater;
		public float plantGrowingTime;
        public int plantGrowingStage;
        public float plantElapsedTimeSinceLastGrowth;


        public GridCellData((int z, int x) cellIndex, PlantObject plantObject)
		{
			GridPlantData plantData = plantObject.GetPlantData(); // Grab data about the plant object

			// Set cell data
			this.cellZ = cellIndex.z;
			this.cellX = cellIndex.x;
			
			// Set plant object data
			this.prefabName = plantData.objectPrefab.name;
			this.plantHealth = plantObject.GetPlantHealth();
			this.plantWater = plantObject.GetPlantWaterLevel();
			this.plantGrowingTime = plantObject.GetPlantCurrentGrowingTime();
			this.plantGrowingStage = plantObject.GetPlantCurrentGrowingStage();
			this.plantElapsedTimeSinceLastGrowth = plantObject.GetPlantElapsedTimeSinceLastGrowth();

		}
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

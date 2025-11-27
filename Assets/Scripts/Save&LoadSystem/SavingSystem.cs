using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Overlays;
using UnityEngine;

public static class SavingSystem
{
    static string saveFileName = "SaveData.json";
    static string saveFilePath = Application.persistentDataPath + "/" + saveFileName;

    public static void SaveGameData(SaveData saveData)
    {
		// Saving a file with all the data
		string jsonData = JsonUtility.ToJson(saveData, true);
		File.WriteAllText(saveFilePath, jsonData);
	}

    public static void LoadSaveFile()
    {
		// Check if a save file exists
		string saveData;
		if (File.Exists(saveFilePath))
		{
			// Load the data from the existing file
			saveData = File.ReadAllText(saveFilePath);
			SaveData loadedData = JsonUtility.FromJson<SaveData>(saveData);

			// Make sure that the data exists
			if (loadedData != null)
			{
				// Make sure that we have a grid system to work with
				if (GameManager.gridSystem != null)
				{
					LoadGridCells(loadedData.occupiedGridCells);
				}

				LoadInventory(loadedData.inventoryData);
			}
		}
	}

	static void LoadGridCells(List<SaveData.GridCellData> gridCells)
	{
		// Spawn any grid objects from previous playthrough
		if (gridCells != null && gridCells.Count > 0)
		{
			foreach (SaveData.GridCellData gridCellData in gridCells)
			{
				// Get a grid cell from the grid system based on the cell data from the save file
				(int z, int x) cellLocation = (gridCellData.cellZ, gridCellData.cellX); // Location of the cell on the grid system
				GridCell gridCellFromData = GameManager.gridSystem.GetGridCellFromLocation(cellLocation);

				// Load the object the grid cell has been occupied with in the previous playthrough
				GameObject gridObjectPrefab = Resources.Load<GameObject>(gridCellData.prefabName);

				PlantObject plantObject = gridObjectPrefab.GetComponent<PlantObject>();
				GridObjectData plantData = plantObject.GetPlantData();

				// If we have sucessfully spawned the grid object
				if (GameManager.gridSystem.SpawnGridObject(gridCellFromData, plantData))
				{
					// Check if it's a plant object, and if it is, update the plant's data with the saved one
					PlantObject cellPlantObject = gridCellFromData.storedGridObject.GetComponent<PlantObject>();
					cellPlantObject.SetPlantHealth(gridCellData.plantHealth);
					cellPlantObject.SetPlantWaterLevel(gridCellData.plantWater);
					cellPlantObject.SetPlantCurrentGrowingTime(gridCellData.plantGrowingTime);
				}
			}
		}
	}

	static void LoadInventory(List<SaveData.InventoryData> inventoryData)
	{
		if (GameManager.player == null) return; // No player exists, so they can't have an inventory

		// Load the player's inventory based on the data stored in the save file
		if (inventoryData != null && inventoryData.Count > 0)
		{
			// Return all the grid object data objects in the assets of the game
			GridObjectData[] gridObjectDataArray = Resources.LoadAll<GridObjectData>("");

			foreach (SaveData.InventoryData data in inventoryData)
			{
				GridObjectData inventoryObjectData = GetCorrectGridObjectDataFromArray(data.prefabName, gridObjectDataArray);
				GameManager.player.GetInventory().AddItem(inventoryObjectData, data.quantity);

			}
		}
	}

	static GridObjectData GetCorrectGridObjectDataFromArray(string prefabNameToCheck, GridObjectData[] gridObjectDataArray)
	{
		if (gridObjectDataArray != null && gridObjectDataArray.Length > 0)
		{
			foreach (GridObjectData gridObjectData in gridObjectDataArray)
			{
				if (prefabNameToCheck == gridObjectData.objectPrefab.name)
				{
					return gridObjectData;
				}
			}
		}

		return null;
	}
}

using Mono.Cecil;
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
				// Make sure that we have a game manager and grid system to work with
				if (GameManager.instance != null && GameManager.gridSystem != null)
				{
					// Spawn any grid objects from previous playthrough
					if (loadedData.occupiedGridCells != null && loadedData.occupiedGridCells.Count > 0)
					{
						foreach (SaveData.GridCellData gridCellData in loadedData.occupiedGridCells)
						{
							// Get a grid cell from the grid system based on the cell data from the save file
							(int z, int x) cellLocation = (gridCellData.cellZ, gridCellData.cellX); // Location of the cell on the grid system
							GridCell gridCellFromData = GameManager.gridSystem.GetGridCellFromLocation(cellLocation);

							// Load the object the grid cell has been occupied with in the previous playthrough
							GameObject gridObjectPrefab = Resources.Load<GameObject>(gridCellData.prefabName);

							PlantObject plantObject = gridObjectPrefab.GetComponent<PlantObject>();
							GridObjectData plantData = plantObject.GetPlantData();

							GameManager.gridSystem.SpawnGridObject(gridCellFromData, plantData);
						}
					}
				}
			}
		}
	}
}

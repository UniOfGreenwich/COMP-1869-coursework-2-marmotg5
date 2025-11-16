using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using static SaveData;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static GridSystem gridSystem {  get; private set; }
    public static UIManager UIManager{ get; private set; }
    public static Camera mainCamera { get; private set; }
    public static Player player { get; private set; }
    //public static SavingSystem savingSystem { get; private set; }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }

        mainCamera = Camera.main;
        player = FindFirstObjectByType<Player>();
        gridSystem = FindFirstObjectByType<GridSystem>();
        UIManager = FindFirstObjectByType<UIManager>();

		// Load the game file
		SavingSystem.LoadSaveFile();
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

	}

	// Update is called once per frame
	void Update()
    {
    }

    
    // On game quit
	void OnApplicationQuit()
	{
        SaveData saveData = new SaveData();  // Create a new sava data file for the player
		PopulateSaveData(saveData); // Populate the sava data file with all the current game data

        // Call a function to the saving system to save all that data to a file on the player's device
        SavingSystem.SaveGameData(saveData);
	}

	void PopulateSaveData(SaveData saveData)
    {
        // Save player data
        if (player != null)
        {
            saveData.playerCash = player.GetCash();
            saveData.playerLevel = player.GetLevel();
            saveData.inventorySystem = player.GetInventory();
        }

        // Save grid system data
        if (gridSystem != null)
        {
            // Get all the occupied cells in the grid system
            List<GridCell> occupiedGridCells = gridSystem.GetAllOccupiedGridCells();
            if (occupiedGridCells != null && occupiedGridCells.Count > 0)
            {
                // Remove occupied cells that store a reference to the same grid object
                List<GridCell> cleanedOccupiedGridCells = GetCleanListFromDuplicateCells(occupiedGridCells);
                if (cleanedOccupiedGridCells != null && cleanedOccupiedGridCells.Count > 0)
                {
					foreach (GridCell gridCell in cleanedOccupiedGridCells)
					{
						GridPlantData plantData = gridSystem.GetPlantDataFromGridCell(gridCell);
						if (plantData != null)
						{
                            // Create a new grid cell data structure to save onto the file
							GridCellData gridCellData = new GridCellData(gridCell.GetCellIndex(), plantData.objectPrefab.name);
							saveData.occupiedGridCells.Add(gridCellData);
						}
					}
				}
			}
		}
    }

    // Removes any occupied grid cells from the list that are occupied by the same plant, resulting in saving only 1 instance of the grid cell
    List<GridCell> GetCleanListFromDuplicateCells(List<GridCell> occupiedGridCellList)
    {
        List<GridCell> cleanList = new List<GridCell>();

        // Loop through all the occupied grid cells in the grid system
        foreach(GridCell cell in occupiedGridCellList)
        {
            if (HasDuplicatedOccupiedCells(cell, cleanList)) continue; // Skip iteration if there are any cells that store the same game object reference
			cleanList.Add(cell); // Add the cell to the clean list if no other instance was found with a similar reference
		}

		return cleanList;
    }

	// Checks if there are any cells that store the same game object reference
	bool HasDuplicatedOccupiedCells(GridCell gridCell, List<GridCell> gridCellList)
    {
        foreach (GridCell currentCell in gridCellList)
        {
            if (currentCell.storedGridObject == gridCell.storedGridObject) return true;
        }
        return false;
    }
}

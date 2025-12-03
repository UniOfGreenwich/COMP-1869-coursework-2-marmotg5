using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static GridSystem gridSystem {  get; private set; }
    public static UIManager UIManager{ get; private set; }
    public static WeatherManager weatherManager { get; private set; }
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
        weatherManager = FindFirstObjectByType<WeatherManager>();

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
		CreateSaveData(); // Create a new save data file every time the game is quit
	}

	void CreateSaveData()
    {
		SaveData saveData = new SaveData();  // Create a new sava data file for the player

		// Save player data
		if (player != null)
        {
            saveData.playerCash = player.GetCash();
            saveData.playerLevel = player.GetLevelAmount();
            saveData.playerLevelExperience = player.GetLevel().experience;

            // Save the player's inventory
            foreach (InventoryItem inventoryItem in player.GetInventory().GetItems())
            {
                SaveData.InventoryData data = new SaveData.InventoryData(inventoryItem);
			    saveData.inventoryData.Add(data);
            }
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
                        PlantObject gridPlantObject = gridSystem.GetPlantObjectFromGridCell(gridCell);
                        if (gridPlantObject != null)
                        {
							GridPlantData plantData = gridPlantObject.GetPlantData();
							if (plantData != null)
							{
								// Create a new grid cell data structure to save onto the file
								SaveData.GridCellData gridCellData = new SaveData.GridCellData(gridCell.GetCellIndex(), gridPlantObject);
								saveData.occupiedGridCells.Add(gridCellData);
							}
						}
					}
				}
			}
		}

		// Call a function to the saving system to save all that data to a file on the player's device
		SavingSystem.SaveGameData(saveData);
	}

    // Removes any occupied grid cells from the list that are occupied by the same plant, resulting in saving only 1 instance of the grid cell
    List<GridCell> GetCleanListFromDuplicateCells(List<GridCell> occupiedGridCellList)
    {
        List<GridCell> cleanList = new List<GridCell>();

        // Loop through all the occupied grid cells in the grid system
        foreach(GridCell currentCell in occupiedGridCellList)
        {
            PlantObject currentCellPlantObject = currentCell.storedGridObject.GetComponent<PlantObject>();
            GridCell objectParentCell = currentCellPlantObject.GetObjectParentGridCell();
            if (currentCell != objectParentCell) continue;

			cleanList.Add(objectParentCell); // Add the cell to the clean list if no other instance was found with a similar reference
		}

		return cleanList;
    }
}

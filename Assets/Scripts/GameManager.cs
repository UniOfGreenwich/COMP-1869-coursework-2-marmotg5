using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static GridSystem gridSystem {  get; private set; }
    public static UIManager UIManager{ get; private set; }
    public static Camera mainCamera { get; private set; }
    public static Player player { get; private set; }
    public static SavingSystem savingSystem { get; private set; }

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
		savingSystem = FindFirstObjectByType<SavingSystem>();
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
        savingSystem.SaveGameData(saveData);
		savingSystem.ReadSaveFile();

	}

	void PopulateSaveData(SaveData saveData)
    {
        if (player != null)
        {
            saveData.playerCash = player.GetCash();
            saveData.playerLevel = player.GetLevel();
            saveData.inventorySystem = player.GetInventory();
        }

        if (gridSystem != null)
        {
			saveData.occupiedGridCells = new List<GridCell>(gridSystem.GetAllOccupiedGridCells());
		}
    }
}

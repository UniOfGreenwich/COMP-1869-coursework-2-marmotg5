using UnityEngine;

[System.Serializable]
struct PlayerStats
{
	public Level level;
    public int cash;
}

public class Player : MonoBehaviour
{
    [SerializeField]
	PlayerStats playerStats;

    PlayerBuilding playerBuilding;
	CameraControl cameraControl;
    InventorySystem inventorySystem = new InventorySystem();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBuilding = GetComponent<PlayerBuilding>();
        cameraControl = GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
    }

	// Used when loading the player's data (SavingSystem.cs)
	public void SetLevel(Level levelToOverwrite)
	{
		playerStats.level = levelToOverwrite;
	}


	public void SetCash(int cash)
	{
		if (cash >= 0)
		{
			playerStats.cash = cash;
		}
		else
		{
			playerStats.cash = 0;
		}
	}

	public void AddCash(int cashToAdd)
	{
		playerStats.cash += cashToAdd;
	}

	public void RemoveCash(int cashToRemove)
	{
		playerStats.cash -= cashToRemove;
	}

	public int GetLevelAmount() { return playerStats.level.levelAmount; }
    public Level GetLevel() { return playerStats.level; }
    public int GetCash() { return playerStats.cash; }

	public InventorySystem GetInventory() { return inventorySystem; }
}

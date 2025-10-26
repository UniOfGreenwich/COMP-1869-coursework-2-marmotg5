using UnityEngine;

[System.Serializable]
struct PlayerStats
{
    public int level;
    public int cash;
}

public class Player : MonoBehaviour
{
    [SerializeField]
	PlayerStats playerStats;

    PlayerBuilding playerBuilding;
	CameraControl cameraControl;
    InventorySystem inventorySystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBuilding = GetComponent<PlayerBuilding>();
        cameraControl = GetComponent<CameraControl>();
        inventorySystem = GetComponent<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

	public void SetLevel(int level)
	{
		if (level >= 0)
		{
			playerStats.level = level;
		}
		else
		{
			playerStats.level = 0;
		}
	}

	public void AddLevel(int levelToAdd)
	{
		playerStats.level += levelToAdd;
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

	public int GetLevel() { return playerStats.level; }
	public int GetCash() { return playerStats.cash; }
}

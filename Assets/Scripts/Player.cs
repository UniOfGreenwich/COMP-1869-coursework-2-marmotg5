using UnityEngine;

[System.Serializable]
struct PlayerStats
{
    int level;
    int cash;

    public void SetLevel(int level)
    {        
        if (level >= 0)
        {
			this.level = level;
		}
        else
        {
            this.level = 0;
        }
    }
    
    public void SetCash(int cash)
    {
        if (cash >= 0)
        {
			this.cash = cash;
		}
		else
		{
			this.cash = 0;
		}
	}
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
}

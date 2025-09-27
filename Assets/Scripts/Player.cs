using UnityEngine;

[System.Serializable]
struct PlayerStats
{
    int level;
    float cash;

    public void SetLevel(int level)
    {        
        if (level >= 0)
        {
			this.level = level;
		}
    }
    
    public void SetCash(float cash)
    {
        if (cash >= 0.0f)
        {
			this.cash = cash;
		}
    }
}

public class Player : MonoBehaviour
{
    [SerializeField]
	PlayerStats playerStats;

	CameraControl cameraControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraControl = GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

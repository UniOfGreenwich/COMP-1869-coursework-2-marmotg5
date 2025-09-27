using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GridSystem gridSystem;
    public static Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        mainCamera = Camera.main;
        gridSystem = FindFirstObjectByType<GridSystem>();

	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
    {
    }

    public static GridSystem GetGridSystem() {return gridSystem;}
}

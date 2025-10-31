using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static GridSystem gridSystem {  get; private set; }
    public static UIManager UIManager{ get; private set; }
    public static Camera mainCamera { get; private set; }
    public static Player player { get; private set; }

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
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
    {
    }
}

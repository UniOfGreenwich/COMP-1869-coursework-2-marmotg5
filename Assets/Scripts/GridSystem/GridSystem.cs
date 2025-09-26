using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    static GridSystem instance;

    [SerializeField]
    int gridCellWidthAmount = 25, gridCellHeightAmount = 25;
    [SerializeField]
    float gridCellSize = 1.0f; // 1 meter unit in Unity 

    //int[,] gridArray;
    
    Grid grid;

    Dictionary<(int x, int y), GridCell> gridArray = new Dictionary<(int x, int y), GridCell>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        grid = GetComponent<Grid>();
        InitializeGrid();
    }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
    {
		GridCell testCell = gridArray[(0, 5)];
		print(testCell.GetCenteredPosition());
	}


    void InitializeGrid()
    {
		gridArray = new Dictionary<(int x, int y), GridCell>(gridCellWidthAmount * gridCellHeightAmount);

		for (int z = 0; z < gridCellWidthAmount; z++)
        {
            for (int x = 0; x < gridCellHeightAmount; x++)
            {
                Vector3 cellPosition = new Vector3(x, transform.position.y, z) * gridCellSize;

                gridArray.Add((z, x), new GridCell((z, x), gridCellSize, cellPosition));
                //print("Grid Cell [" + "0" + "] at Coords: " + new Vector3(x, transform.position.y, z) * gridCellSize);
            }
        }
    }

    public static GridSystem GetInstance() {  return instance; }
    public Dictionary<(int x, int y), GridCell> GetGridCells() { return gridArray; }
}

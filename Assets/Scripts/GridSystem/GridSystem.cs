using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public enum GridObjectType
{
	DECORATION,
	PLANT
}

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance { get; private set; }
    [SerializeField]
    GameObject gridCellVisualPrefab;

	[SerializeField]
    int gridCellWidthAmount = 25, gridCellHeightAmount = 25;
    [SerializeField]
    float gridCellSize = 1.0f; // 1 meter unit in Unity 

    Dictionary<(int z, int x), GridCell> gridArray = new Dictionary<(int z, int x), GridCell>();
    List<GameObject> gridCellVisuals = new List<GameObject>();

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

        //grid = GetComponent<Grid>();
        InitializeGrid();
    }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update()
    {

	}


    void InitializeGrid()
    {
		gridArray = new Dictionary<(int z, int x), GridCell>(gridCellWidthAmount * gridCellHeightAmount);

		for (int x = 0; x < gridCellHeightAmount; x++)
        {
            for (int z = 0; z < gridCellWidthAmount; z++)
            {
                Vector3 cellPosition = new Vector3(x * gridCellSize, transform.position.y, z * gridCellSize);
                GridCell gridCell = new GridCell((z, x), gridCellSize, cellPosition);


                //gridArray.Add((z, x), new GridCell((z, x), gridCellSize, cellPosition));

                gridArray.Add((z, x), gridCell);

                // Adding the grid cell visual sprite stuff to a list that we can access later
                GameObject gridCellVisual = Instantiate(gridCellVisualPrefab, gridCell.GetCenteredPosition() + gridCellVisualPrefab.transform.position, gridCellVisualPrefab.transform.rotation, transform);
                gridCellVisual.name = "GridCellVisual_" + z + x;
                gridCellVisuals.Add(gridCellVisual);
            }
        }

        SetGridSystemRendering(false); // Don't render grid by default
		//RenderGrid();
	}

    public void SetGridSystemRendering(bool toggle)
    {
		foreach (GameObject visualObject in gridCellVisuals)
		{
			SpriteRenderer spriteRenderer = visualObject.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null)
			{
				visualObject.SetActive(toggle);
			}
		}
	}

  //  void RenderGrid()
  //  {
		//for (int z = 0; z < gridCellWidthAmount; z++)
		//{
		//	GridCell firstCell = gridArray[(z, 0)];
		//	GridCell lastCell = gridArray[(z, gridCellHeightAmount - 1)];
		//	Debug.DrawLine(firstCell.GetCellCornerPosition(), lastCell.GetCellCornerPosition(), Color.white, 100.0f, true);
		//}

  //      for (int x = 0; x < gridCellHeightAmount; x++)
  //      {
  //          GridCell firstCell = gridArray[(0, x)];
  //          GridCell lastCell = gridArray[(gridCellWidthAmount - 1, x)];
  //          Debug.DrawLine(firstCell.GetCellCornerPosition(), lastCell.GetCellCornerPosition(), Color.white, 100.0f, true);
  //      }
  //  }

    public GridCell GetGridCellFromCoords(Vector3 coords)
    {
        // Loop through each grid cell and check if the coords are within a certain grid cell's perimeter
        foreach (KeyValuePair<(int z, int x), GridCell> search in gridArray)
        {
            GridCell gridCell = search.Value;
            bool foundCell = gridCell.IsPointInsideGridCell(coords);


            if (foundCell) {return gridCell;}
		}

        return null;
    }

    public Dictionary<(int z, int x), GridCell> GetGridArray() { return gridArray; }
}

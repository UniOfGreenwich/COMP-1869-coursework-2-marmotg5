using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance { get; private set; }
	LineRenderer lineRenderer;

    [SerializeField]
    int gridCellWidthAmount = 25, gridCellHeightAmount = 25;
    [SerializeField]
    float gridCellSize = 1.0f; // 1 meter unit in Unity 

    Grid grid;

    Dictionary<(int z, int x), GridCell> gridArray = new Dictionary<(int z, int x), GridCell>();

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
        lineRenderer = GetComponent<LineRenderer>();
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

		for (int z = 0; z < gridCellWidthAmount; z++)
        {
            for (int x = 0; x < gridCellHeightAmount; x++)
            {
                Vector3 cellPosition = new Vector3(x, transform.position.y, z) * gridCellSize;
                gridArray.Add((z, x), new GridCell((z, x), gridCellSize, cellPosition));
            }
        }

        RenderGrid();
    }

    void RenderGrid()
    {
		lineRenderer.positionCount = gridCellWidthAmount + gridCellHeightAmount;

		for (int z = 0; z < gridCellWidthAmount; z++)
		{
			GridCell firstCell = gridArray[(z, 0)];
			GridCell lastCell = gridArray[(z, gridCellHeightAmount - 1)];
			Debug.DrawLine(firstCell.GetCellCornerPosition(), lastCell.GetCellCornerPosition(), Color.white, 100.0f, true);
		}

        for (int x = 0; x < gridCellHeightAmount; x++)
        {
            GridCell firstCell = gridArray[(0, x)];
            GridCell lastCell = gridArray[(gridCellWidthAmount - 1, x)];
            Debug.DrawLine(firstCell.GetCellCornerPosition(), lastCell.GetCellCornerPosition(), Color.white, 100.0f, true);
        }



        //for (int z = 0;z < gridCellWidthAmount; z++)
        //{
        //    for (int x = 0; x < gridCellHeightAmount; x++)
        //    {
        //        GridCell cell = gridArray[(z, x)];
        //        Vector3 heightDireciton = new Vector3(cell.GetCellCornerPosition().x, cell.GetCellCornerPosition().y * 100.0f, cell.GetCellCornerPosition().z);
        //        Debug.DrawLine(cell.GetCellCornerPosition(), heightDireciton, Color.red, 100.0f, true);
        //    }
        //}


        //      for (int z = 0; z < gridCellWidthAmount; z++)
        //      {
        //          //Vector3 firstCell = gridArray[(z, 0)].GetCellCornerPosition();
        //	Vector3 lastCell = gridArray[(z, gridCellWidthAmount - 1)].GetCellCornerPosition();
        //          lineRenderer.SetPosition(z, lastCell);
        //}

        //for (int x = 0; x < gridCellWidthAmount; x++)
        //{
        //	//Vector3 firstCell = gridArray[(0, x)].GetCellCornerPosition();
        //	Vector3 lastCell = gridArray[(x, gridCellHeightAmount - 1)].GetCellCornerPosition();
        //	lineRenderer.SetPosition(x, lastCell);
        //}
        //      for (int z = 0; z < gridCellWidthAmount; z++)
        //      {
        //          for (int x = 0; x < gridCellHeightAmount; x++)
        //          {
        //              Vector3 lineRenderPositionX = gridArray[(x, z)].GetCellCornerPosition();
        //              lineRenderer.SetPosition(x, lineRenderPositionX);
        //	}
        //}    

        //          print("Linerenderer pos count: " + lineRenderer.positionCount);
    }

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

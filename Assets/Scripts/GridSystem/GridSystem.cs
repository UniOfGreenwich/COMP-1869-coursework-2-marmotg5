using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public enum GridObjectType
{
	DECORATION,
	PLANT
}

public class GridSystem : MonoBehaviour
{
    const string VISUAL_OBJECT_NAME = "GridCellVisual_";


	public static GridSystem instance { get; private set; }
	[SerializeField]
	GridObjectData[] allowedGridObjects;
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

				GameObject gridCellVisual = Instantiate(gridCellVisualPrefab, gridCell.GetCenteredPosition() + gridCellVisualPrefab.transform.position, gridCellVisualPrefab.transform.rotation, transform);
				gridCellVisual.name = VISUAL_OBJECT_NAME + z + x;
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

    // Checks if the grid cell can be found in the grid system
    bool DoesGridCellExist(GridCell cellToCheck)
    {
		foreach (KeyValuePair<(int z, int x), GridCell> search in gridArray)
		{
			GridCell arrayGridCell = search.Value;
            if (cellToCheck == arrayGridCell)
            {
                return true;
            }
		}
		return false;
    }

    GameObject FindVisualObjectBasedOnCell(GridCell gridCell)
    {
        (int z, int x) gridCellLocation = gridCell.GetCellIndex();

		for (int i = 0; i < gridCellVisuals.Count; i ++)
        {
            GameObject visualObject = gridCellVisuals[i];
            if (visualObject != null)
            {
                if (visualObject.name == VISUAL_OBJECT_NAME + gridCellLocation.z + gridCellLocation.x)
                {
                    return visualObject;
                }
            }
        }

		return null;
    }

	public void SpawnGridObject(GridCell cellToSpawnIn)
	{
		if (allowedGridObjects.Contains(allowedGridObjects[0]))
		{
			if (DoesGridCellExist(cellToSpawnIn))
			{
				GameObject gridObjectPrefab = allowedGridObjects[0].objectPrefab;
				GameObject visualObjectForCell = FindVisualObjectBasedOnCell(cellToSpawnIn);

				Vector3 spawnPosition = cellToSpawnIn.GetCenteredPosition() + gridObjectPrefab.transform.position;

				Instantiate(gridObjectPrefab, spawnPosition, gridObjectPrefab.transform.rotation, visualObjectForCell.transform);
			}
		}
	}

	public void SpawnGridObject(GridCell cellToSpawnIn, GridObjectData gridObjectData)
    {
		if (allowedGridObjects.Contains(gridObjectData))
		{
			if (DoesGridCellExist(cellToSpawnIn))
            {
                GameObject gridObjectPrefab = gridObjectData.objectPrefab;
                GameObject visualObjectForCell = FindVisualObjectBasedOnCell(cellToSpawnIn);

                Vector3 spawnPosition = cellToSpawnIn.GetCenteredPosition() + gridObjectPrefab.transform.position;

				Instantiate(gridObjectPrefab, spawnPosition, gridObjectPrefab.transform.rotation, visualObjectForCell.transform);
            }
		}
	}

    //// Check if the grid object we want to spawn is allowed within the grid system
    //bool IsGridObjectAllowed(GridObjectData gridObjectData)
    //{
    //    if (allowedGridObjects.Contains(gridObjectData))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    public Dictionary<(int z, int x), GridCell> GetGridArray() { return gridArray; }
}

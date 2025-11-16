using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Mathematics;
using System.Runtime.InteropServices.WindowsRuntime;

public enum GridObjectType
{
	DECORATION,
	PLANT
}

public class GridSystem : MonoBehaviour
{
	const string VISUAL_OBJECT_NAME = "GridCellVisual_";

	public static GridSystem instance { get; private set; }
	[SerializeField] GameObject gridCellVisualPrefab;
	Color gridCellVisualDefaultColor; // The default color of the grid cell's visual prefab sprite renderer

	[SerializeField] int gridCellWidthAmount = 25, gridCellHeightAmount = 25;
	[SerializeField] float gridCellSize = 1.0f; // 1 meter unit in Unity 

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

		// Grab the default color of the grid cell visual object from its prefab
		if (gridCellVisualPrefab != null)
			gridCellVisualDefaultColor = gridCellVisualPrefab.GetComponent<SpriteRenderer>() ? gridCellVisualPrefab.GetComponent<SpriteRenderer>().color : Color.white;

		// Create the grid
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
				// Calculate the position of the new grid cell
				Vector3 cellPosition = new Vector3(x * gridCellSize, transform.position.y, z * gridCellSize);
				GridCell gridCell = new GridCell((z, x), gridCellSize, cellPosition);

				gridArray.Add((z, x), gridCell);

				// Create a visual object to represent the grid cell
				GameObject gridCellVisual = Instantiate(gridCellVisualPrefab, gridCell.GetCenteredPosition() + gridCellVisualPrefab.transform.position, gridCellVisualPrefab.transform.rotation, transform);
				gridCellVisual.name = VISUAL_OBJECT_NAME + z + x;
				gridCellVisuals.Add(gridCellVisual);
			}
		}

		SetGridSystemRendering(false); // Don't render grid by default
	}

	public void SetGridSystemRendering(bool toggle)
	{
		// Show/hide the sprite renderer for each grid cell visual object
		foreach (GameObject visualObject in gridCellVisuals)
		{
			SpriteRenderer spriteRenderer = visualObject.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = toggle;

			}
		}
	}

	public bool SpawnGridObject(GridCell cellToSpawnIn, GridObjectData gridObjectData)
	{
		if (DoesGridCellExist(cellToSpawnIn))
		{
			Vector2Int gridObjectCellRequirements = gridObjectData.gridCellRequirement;

			// Check if the grid object we are trying to place has enough space to be spawned on the grid
			if (IsGridCellAreaClear(cellToSpawnIn, gridObjectCellRequirements))
			{
				GameObject gridObjectPrefab = gridObjectData.objectPrefab;

				// Calculate the object's spawn position
				GameObject visualObjectForCell = FindVisualObjectBasedOnCell(cellToSpawnIn);
				Vector3 spawnPosition = GetObjectSpawnPosition(cellToSpawnIn, gridObjectPrefab);

				// Create the grid object
				GameObject spawnedGridObject = Instantiate(gridObjectPrefab, spawnPosition, gridObjectPrefab.transform.rotation, visualObjectForCell.transform);
				GridObject spawnedGridObjectData = spawnedGridObject.GetComponent<GridObject>();
				if (spawnedGridObjectData != null)
				{
					// Set the spawned grid object to keep track of the parent cell its placed in
					spawnedGridObjectData.UpdateParentCell(cellToSpawnIn);

					// Set each necessary grid cell to keep track of the spawned grid object that may occupy more than 1x1 cells
					Dictionary<(int z, int x), GridCell> gridCellsInArea = GetGridCellsInArea(cellToSpawnIn, gridObjectCellRequirements);
					OccupyCellsWithObject(gridCellsInArea, spawnedGridObject);

					return true;
				}
			}
		}

		return false;
	}

	public GridCell GetGridCellFromCoords(Vector3 coords)
	{
		// Loop through each grid cell and check if the coords are within a certain grid cell's perimeter
		foreach (KeyValuePair<(int z, int x), GridCell> search in gridArray)
		{
			GridCell gridCell = search.Value;
			bool foundCell = gridCell.IsPointInsideGridCell(coords);


			if (foundCell) { return gridCell; }
		}

		return null;
	}

	public GridCell GetGridCellFromLocation((int z, int x) locationToCheck)
	{
		// Loop through each grid cell and check if the coords are within a certain grid cell's perimeter
		foreach (KeyValuePair<(int z, int x), GridCell> search in gridArray)
		{
			GridCell gridCell = search.Value;
			(int z, int x) gridCellLocation = gridCell.GetCellIndex();

			if (gridCellLocation == locationToCheck) { return gridCell; }
		}

		return null;
	}

	public GridPlantData GetPlantDataFromGridCell(GridCell gridCell)
	{
		if (gridCell == null || gridCell.storedGridObject == null) return null;

		PlantObject plantObject = gridCell.storedGridObject.GetComponent<PlantObject>();
		if (plantObject != null)
		{
			return plantObject.GetPlantData();
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

	// Checks if any grid cell is occupied within a specified range
	public bool IsGridCellAreaClear(GridCell gridCell, Vector2Int gridCellRequirements) // x = Z | y = X
	{
		Dictionary<(int z, int x), GridCell> gridCellsInArea = GetGridCellsInArea(gridCell, gridCellRequirements);

		foreach (KeyValuePair<(int z, int x), GridCell> search in gridCellsInArea)
		{
			GridCell arrayGridCell = search.Value;
			if (arrayGridCell != null)
			{
				if (arrayGridCell.IsOccupied())
				{
					return false;
				}
			}
		}

		return true;
	}

	// Returns a dictionary of all grid cells around the "targetCell" with a set cell location radius
	public Dictionary<(int z, int x), GridCell> GetGridCellsInArea(GridCell targetCell, Vector2Int targetCellSizeRequierements)
	{
		Dictionary<(int z, int x), GridCell> gridCellsInArea = new Dictionary<(int z, int x), GridCell>();

		// Get the location of the target cell in the grid system
		(int z, int x) targetCellLocation = targetCell.GetCellIndex();

		// Locations in the grid system that we will loop to
		int widthMax = targetCellLocation.z + targetCellSizeRequierements.x;
		int heightMax = targetCellLocation.x - targetCellSizeRequierements.y;

		// Grab each cell that is within those locations in the grid ystem
		for (int x = targetCellLocation.x; x > heightMax; x--)
		{
			for (int z = targetCellLocation.z; z < widthMax; z++)
			{
				(int z, int x) currentLocation = (z, x);

				GridCell cellToCheck = GetGridCellFromLocation((z, x));
				if (cellToCheck != null)
				{
					gridCellsInArea.Add(currentLocation, cellToCheck);
				}
			}
		}

		return gridCellsInArea;
	}

	public GameObject FindVisualObjectBasedOnCell(GridCell gridCell)
	{
		(int z, int x) gridCellLocation = gridCell.GetCellIndex(); // The location of the grid cell on the grid system

		for (int i = 0; i < gridCellVisuals.Count; i++)
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

	public List<GameObject> FindVisualObjectsBasedOnCell(GridCell targetCell, Vector2Int targetCellRequirements)
	{
		// Grab the actual grid cells using the target cell and its size requirements
		Dictionary<(int z, int x), GridCell> gridCellsInArea = GetGridCellsInArea(targetCell, targetCellRequirements);
	
		if (gridCellsInArea.Count > 0)
		{
			List<GameObject> cellVisualObjects = new List<GameObject>();

			// Loop through the grid cells
			foreach (KeyValuePair<(int z, int x), GridCell> search in gridCellsInArea)
			{
				// Grab the visual object for the grid cell
				GameObject cellVisualObject = FindVisualObjectBasedOnCell(search.Value);
				if (cellVisualObject != null)
				{
					cellVisualObjects.Add(cellVisualObject);
				}
			}

			return cellVisualObjects;
		}

		return null;
	}

	// Returns a list of all grid cells that have objects placed on top of the cells
	public List<GridCell> GetAllOccupiedGridCells()
	{
		List<GridCell> occupiedCells = new List<GridCell>();

		// Loop through the grid cells in the grid system
		foreach (KeyValuePair<(int z, int x), GridCell> search in gridArray)
		{
			if (!search.Value.IsOccupied()) continue; // Ignore the grid cell if it's not occupied
			
			occupiedCells.Add(search.Value);
		}

		return occupiedCells;
	}

	// Set grid cells to be occupied with a certain game object (disallowing other game/grid objects to be spawned on the grid cells)
	void OccupyCellsWithObject(Dictionary<(int z, int x), GridCell> parentCells, GameObject occupyingObject)
	{
		foreach (KeyValuePair<(int z, int x), GridCell> search in parentCells)
		{
			GridCell arrayGridCell = search.Value;
			if (arrayGridCell != null)
			{
				arrayGridCell.StoreGridObject(occupyingObject);
			}
		}
	}

	// Function used in PlayerBuildingItemUI.cs as well
	// Gets the location of where the prefab object should spawn on top of the specified grid cell
	public Vector3 GetObjectSpawnPosition(GridCell gridCell, GameObject prefabObject)
	{
		return gridCell.GetCenteredPosition() + prefabObject.transform.position;
	}

    public Dictionary<(int z, int x), GridCell> GetGridArray() { return gridArray; }

	public Color GetGridCellVisualDefaultColor() { return gridCellVisualDefaultColor; }
}

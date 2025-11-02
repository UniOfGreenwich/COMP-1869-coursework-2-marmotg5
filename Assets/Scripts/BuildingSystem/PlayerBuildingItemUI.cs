using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerBuildingItemUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("UI Prefixes")]
    [SerializeField] string quantityPrefix = "Quantity: ";

    [Header("UI Text")]
	[SerializeField] TextMeshProUGUI itemNameText;
	[SerializeField] TextMeshProUGUI itemQuantityText;

	[Header("UI Image")]
	[SerializeField] Image itemImage;

    // The data
    InventoryItem inventoryItem = null;

	// The placeholder object that the player will see before spawning in the ACTUAL/REAL thing (will show if cells are occupied or empty) 
	GameObject placeHolderObject = null;
    GridCell placeHolderObjectGridCell = null; // The grid cell the "placeHolderObject" is currently on top of

    List<GameObject> colouredGridCellVisualObjects = new List<GameObject>();

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    // Gets called when the player drags the UI element
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // Make sure we have all necessary data ready before trying to do anything else
        if (GameManager.mainCamera == null || GameManager.gridSystem == null || inventoryItem == null) return;

        // Fire a 3D ray into the world to see where the player is trying to drag their mouse/finger
        Ray ray = GameManager.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            // If the coordinates we are dragging the item are on an active/existing grid cell
            GridCell gridCellAtCoords = GameManager.gridSystem.GetGridCellFromCoords(raycastHit.point);
            if (gridCellAtCoords != null)
            {
                CreatePlaceholderObject(gridCellAtCoords);
			}

        }

    }

    // Gets called when the player stops dragging the mouse
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.gridSystem == null || 
        placeHolderObject == null || placeHolderObjectGridCell == null) return; // It should never be null since it only executes after the OnDrag event ends but still just in case
        
        ResetColourOfAllGridCells(); // Reset the colour of all grid cell visual objects
		Destroy(placeHolderObject); // Delete the place holder object
        placeHolderObject = null;

        GameManager.gridSystem.SpawnGridObject(placeHolderObjectGridCell);

		// REPLACE THE SpawnGridObject OVERRIDED FUNCTION ABOVE WITH THE OTHER ONE IN THE GridSystem.cs AND MAKE IT SEND THE CURRENT inventoryItem.itemData THROUGH
		// THAT WAY THE ONLY WAY TO SPAWN THINGS IN THE GAME IS THROUGH THE PLAYER BUILDING UI
		// ALSO REMOVE THE DEFAULT WAY OF SPAWNING OBJECTS FROM THE PlayerBuilding.cs


		// REPLACE THE SpawnGridObject OVERRIDED FUNCTION ABOVE WITH THE OTHER ONE IN THE GridSystem.cs AND MAKE IT SEND THE CURRENT inventoryItem.itemData THROUGH
		// THAT WAY THE ONLY WAY TO SPAWN THINGS IN THE GAME IS THROUGH THE PLAYER BUILDING UI
		// ALSO REMOVE THE DEFAULT WAY OF SPAWNING OBJECTS FROM THE PlayerBuilding.cs


		// REPLACE THE SpawnGridObject OVERRIDED FUNCTION ABOVE WITH THE OTHER ONE IN THE GridSystem.cs AND MAKE IT SEND THE CURRENT inventoryItem.itemData THROUGH
		// THAT WAY THE ONLY WAY TO SPAWN THINGS IN THE GAME IS THROUGH THE PLAYER BUILDING UI
		// ALSO REMOVE THE DEFAULT WAY OF SPAWNING OBJECTS FROM THE PlayerBuilding.cs


		// REPLACE THE SpawnGridObject OVERRIDED FUNCTION ABOVE WITH THE OTHER ONE IN THE GridSystem.cs AND MAKE IT SEND THE CURRENT inventoryItem.itemData THROUGH
		// THAT WAY THE ONLY WAY TO SPAWN THINGS IN THE GAME IS THROUGH THE PLAYER BUILDING UI
		// ALSO REMOVE THE DEFAULT WAY OF SPAWNING OBJECTS FROM THE PlayerBuilding.cs
	}

	void CreatePlaceholderObject(GridCell gridCellToSpawnAt)
    {
        if (GameManager.gridSystem == null || inventoryItem == null) return; // Make sure we have a grid system we can create the placeholder object on

        // Create a new placeholder object
        if (placeHolderObject == null)
        {
			// Check if the prefab has a mesh filter and a mesh renderer
			MeshFilter prefabMeshFilter = inventoryItem.itemData.objectPrefab.GetComponentInChildren<MeshFilter>();
			MeshRenderer prefabMeshRenderer = inventoryItem.itemData.objectPrefab.GetComponentInChildren<MeshRenderer>();

			if (prefabMeshFilter != null && prefabMeshRenderer != null)
			{
                // Create the placeholder object
				placeHolderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                placeHolderObject.transform.position = GameManager.gridSystem.GetObjectSpawnPosition(gridCellToSpawnAt, inventoryItem.itemData.objectPrefab);

                // Get the new object's mesh filter and renderer
				MeshFilter placeHolderMeshFilter = placeHolderObject.GetComponent<MeshFilter>();
				MeshRenderer placeHolderRenderer = placeHolderObject.GetComponent<MeshRenderer>();

                // Set the new object's mesh filter and renderer to the ACTUAL prefab's
				placeHolderMeshFilter.mesh = prefabMeshFilter.sharedMesh;
				placeHolderRenderer.material = prefabMeshRenderer.sharedMaterial;

                // Change the new object's material to transparent
                placeHolderRenderer.material.color = new Color(placeHolderRenderer.material.color.r, placeHolderRenderer.material.color.g, placeHolderRenderer.material.color.b, 0.5f);

            }
		}
        // We already have an existing one
        else
        {
            placeHolderObject.transform.position = GameManager.gridSystem.GetObjectSpawnPosition(gridCellToSpawnAt, inventoryItem.itemData.objectPrefab);
			ResetColourOfOldGridCells(gridCellToSpawnAt); // Reset the colours of all the grid cell visual objects after moving the placeholder object to a different location

		}

        placeHolderObjectGridCell = gridCellToSpawnAt;
		ColourCells(gridCellToSpawnAt, inventoryItem.itemData.gridCellRequirement); // Color all the necessary cells around the placeholder object

	}

	void ColourCells(GridCell targetCell, Vector2Int targetCellRequirements)
    {
        if (GameManager.gridSystem == null) return;

        List<GameObject> cellVisualObjects = GameManager.gridSystem.FindVisualObjectsBasedOnCell(targetCell, targetCellRequirements);
        if (cellVisualObjects != null || cellVisualObjects.Count > 0)
        {
            Color colourToChange = Color.red;
            if (GameManager.gridSystem.IsGridCellAreaClear(targetCell, targetCellRequirements))
            {
                colourToChange = Color.green;
            }


            foreach (GameObject cellVisualObject in cellVisualObjects)
            {
                // Ignore any cells that are already coloured and continue to next iteration
                if (colouredGridCellVisualObjects.Contains(cellVisualObject)) continue;

                SpriteRenderer spriteRenderer = cellVisualObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = colourToChange;
                    colouredGridCellVisualObjects.Add(cellVisualObject);
				}
			}
		}

    }

    void ResetColourOfOldGridCells(GridCell ignoreGridCell)
    {
        if (GameManager.gridSystem == null) return;

        // Make sure we have coloured grid cells saved before trying to remove anything
        if (colouredGridCellVisualObjects.Count > 0)
        {
            // This will ignore removing the colour of the main grid cell the player is dragging their mouse/finger on
			GameObject ignoreVisualObject = GameManager.gridSystem.FindVisualObjectBasedOnCell(ignoreGridCell);

            // Loop through all coloured grid cell visual game objects and set them to their default colour
			foreach (GameObject gridCellVisualObject in colouredGridCellVisualObjects)
			{

                if (gridCellVisualObject == ignoreVisualObject) continue;

				SpriteRenderer spriteRenderer = gridCellVisualObject.GetComponent<SpriteRenderer>();
				if (spriteRenderer != null)
				{
					spriteRenderer.color = GameManager.gridSystem.GetGridCellVisualDefaultColor();
				}
			}

            colouredGridCellVisualObjects.Clear();
		}
    }

	void ResetColourOfAllGridCells()
    {
        foreach (GameObject gridCellVisualObject in colouredGridCellVisualObjects)
        {
			SpriteRenderer spriteRenderer = gridCellVisualObject.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null)
			{
				spriteRenderer.color = GameManager.gridSystem.GetGridCellVisualDefaultColor();
			}
		}

        colouredGridCellVisualObjects.Clear();
    }


	public void UpdateInventoryItemUI(InventoryItem updatedInventoryItem)
    {
        if (updatedInventoryItem == null) return;
        inventoryItem = updatedInventoryItem;

        itemNameText.text = inventoryItem.itemData.objectName;
        itemQuantityText.text = quantityPrefix + inventoryItem.quantity.ToString();
        itemImage.sprite = inventoryItem.itemData.objectSprite;
    }

}

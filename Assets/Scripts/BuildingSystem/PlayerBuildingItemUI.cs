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
        placeHolderObject == null || placeHolderObjectGridCell == null || 
        inventoryItem == null) return; // It should never be null since it only executes after the OnDrag event ends but still just in case

        // Reset/remove data and the place holder object
        ResetColourOfAllGridCells(); // Reset the colour of all grid cell visual objects
        Destroy(placeHolderObject); // Delete the place holder object
        placeHolderObject = null;

        // Try to spawn the object onto the grid system
        bool successfulSpawn = GameManager.gridSystem.SpawnGridObject(placeHolderObjectGridCell, inventoryItem.itemData);

        // If the player successfully spawned the plant
        if (successfulSpawn && GameManager.player != null)
        {
            InventorySystem playerInventory = GameManager.player.GetInventory();
            playerInventory.UseItem(inventoryItem);

            UpdateInventoryItemUI(inventoryItem); // Update UI with the latest information

		}
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
		ColourPlaceHolderObjectCells(gridCellToSpawnAt, inventoryItem.itemData.gridCellRequirement); // Color all the necessary cells around the placeholder object
	}

	void ColourPlaceHolderObjectCells(GridCell targetCell, Vector2Int targetCellRequirements)
    {
        if (GameManager.gridSystem == null) return;

        Dictionary<(int z, int x), GridCell> gridCellsNearTargetCell = GameManager.gridSystem.GetGridCellsInArea(targetCell, targetCellRequirements);
        if (gridCellsNearTargetCell != null && gridCellsNearTargetCell.Count > 0)
        {
            foreach (KeyValuePair<(int z, int x), GridCell> search in gridCellsNearTargetCell)
            {
                GameObject gridCellVisualObject = GameManager.gridSystem.FindVisualObjectBasedOnCell(search.Value);
                if (gridCellVisualObject != null)
                {
                    if (search.Value.IsOccupied()) continue;

                    SpriteRenderer visualObjectSpriteRenderer = gridCellVisualObject.GetComponent<SpriteRenderer>();
                    if (visualObjectSpriteRenderer != null)
                    {
                        visualObjectSpriteRenderer.color = Color.green;
                        colouredGridCellVisualObjects.Add(gridCellVisualObject);
                    }
                }
            }
        }
    }

    void ResetColourOfOldGridCells(GridCell targetCell)
    {
        if (GameManager.gridSystem == null) return;

        // Make sure we have coloured grid cells saved before trying to remove anything
        if (colouredGridCellVisualObjects.Count > 0)
        {
            // Loop through all coloured grid cell visual game objects and set them to their default colour
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
    }

    // Reset the colour of all the grid cell visual objects that we have previously coloured
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


    // Called when the object is first created to update with all the necessary information
	public void UpdateInventoryItemUI(InventoryItem updatedInventoryItem)
    {
        if (updatedInventoryItem == null) return;
        if (updatedInventoryItem.quantity <= 0) Destroy(gameObject); // Delete the UI object from the player building menu if there are no more quantities

		inventoryItem = updatedInventoryItem;

        itemNameText.text = inventoryItem.itemData.objectName;
        itemQuantityText.text = quantityPrefix + inventoryItem.quantity.ToString();
        itemImage.sprite = inventoryItem.itemData.objectSprite;

    }

}

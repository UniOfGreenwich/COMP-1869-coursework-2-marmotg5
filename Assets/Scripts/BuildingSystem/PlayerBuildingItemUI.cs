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
        if (placeHolderObject == null) return; // It should never be null since it only executes after the OnDrag ends but still just in case
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

                ColourCells(gridCellToSpawnAt, inventoryItem.itemData.gridCellRequirement);
            }
		}
        // We already have an existing one
        else
        {
            placeHolderObject.transform.position = GameManager.gridSystem.GetObjectSpawnPosition(gridCellToSpawnAt, inventoryItem.itemData.objectPrefab);

		}
    }

    void ColourCells(GridCell targetCell, Vector2Int targetCellRequirements)
    {
        if (GameManager.gridSystem == null) return;

        List<GameObject> cellVisualObjects = GameManager.gridSystem.FindVisualObjectsBasedOnCell(targetCell, targetCellRequirements);
        if (cellVisualObjects != null || cellVisualObjects.Count > 0)
        {
            foreach (GameObject cellVisualObject in cellVisualObjects)
            {
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
				// STORE THE DEFAULT COLOR OF THE VISIUAL OBJECT AND THEN MAKE IT RED OR GREEN DEPENDING ON THE OCCUPATION STATE OF THE GRID CELLS
			}
		}

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

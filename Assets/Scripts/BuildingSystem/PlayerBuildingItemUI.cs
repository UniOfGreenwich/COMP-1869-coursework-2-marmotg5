using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBuildingItemUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("UI Prefixes")]
    [SerializeField] string quantityPrefix = "Quantity: ";

    [Header("UI Text")]
	[SerializeField] TextMeshProUGUI itemNameText;
	[SerializeField] TextMeshProUGUI itemQuantityText;

	[Header("UI Image")]
	[SerializeField] Image itemImage;

    InventoryItem inventoryItem;

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
                //MeshFilter itemPrefabMeshFilter = inventoryItem.itemData.objectPrefab.GetComponentInChildren<MeshFilter>();

                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                primitive.transform.position = gridCellAtCoords.GetCenteredPosition();
                //primitive.GetComponent<MeshFilter>().mesh = itemPrefabMeshFilter.mesh;

                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
                // MAKE IT SO THE NEW PLACEHOLDER GAMEOBJECT SHOWS THE PREFABS MESH AND MATERIAL WITH A BIT OF TRANSPARENCY
            }

        }

    }

    // Gets called when the player stops dragging the mouse
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

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

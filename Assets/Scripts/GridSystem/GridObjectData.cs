using UnityEngine;

public abstract class GridObjectData : ScriptableObject
{
	public string objectName = "Object";
	public GameObject objectPrefab;
	public GridObjectType objectType;
	public Sprite objectSprite;
	public int objectCost = 10; // How much it costs in the shop
	public int objectRequiredLevel = 1; // The required level for the player to be able to own this grid object

	public Vector2Int gridCellRequirement = Vector2Int.one; // Default 1 grid cell per axis

	public abstract GridObjectData GetSelf();

}
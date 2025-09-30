using UnityEngine;

public abstract class GridObjectData : ScriptableObject
{
	public string objectName = "Object";
	public GameObject objectPrefab;
	public GridObjectType objectType;
	public Sprite objectSprite;
	public Vector2Int gridCellRequirement = new Vector2Int(1, 1); // Default 1 grid cell per axis

}
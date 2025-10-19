using UnityEngine;

public abstract class GridObjectData : ScriptableObject
{
	public string objectName = "Object";
	public GameObject objectPrefab;
	public GridObjectType objectType;
	public Sprite objectSprite;
	public Vector2Int gridCellRequirement = Vector2Int.one; // Default 1 grid cell per axis

	public abstract GridObjectData GetSelf();

}
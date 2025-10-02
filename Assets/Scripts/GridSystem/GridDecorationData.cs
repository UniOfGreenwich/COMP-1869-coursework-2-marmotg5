using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Decoration Object", menuName = "Scriptable Objects/Grid/Grid Decoration Data")]
public class GridDecorationData : GridObjectData
{
	private void OnEnable()
	{
		objectType = GridObjectType.DECORATION;
	}

	public override GridObjectData GetSelf()
	{
		return this;
	}
}

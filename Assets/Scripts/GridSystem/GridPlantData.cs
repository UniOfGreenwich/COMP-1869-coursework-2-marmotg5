using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Plant Object", menuName = "Scriptable Objects/Grid/Grid Plant Data")]
public class GridPlantData : GridObjectData
{
	[Range(1, 3)]
	public int growingStages = 3;
	public int cashReward = 100;

	public Mesh[] growingStageMeshes;

	private void OnEnable()
	{
		objectType = GridObjectType.PLANT;
	}

	public override GridObjectData GetSelf()
	{
		return this;
	}
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Plant Object", menuName = "Scriptable Objects/Grid/Grid Plant Data")]
public class GridPlantData : GridObjectData
{
	[Range(0, 3)]
	public int growingStages = 3;
	public int cashReward = 100;
	public float requiredGrowingTime = 10.0f; // In seconds 

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

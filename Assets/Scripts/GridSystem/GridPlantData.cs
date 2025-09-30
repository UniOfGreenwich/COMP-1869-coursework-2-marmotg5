using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Plant Object", menuName = "Scriptable Objects/Grid/Grid Plant Data")]
public class GridPlantData : GridObjectData
{
	public int growingStages = 3;
	public float cashReward = 100.00f;

	public Mesh[] growingStageMeshes;

	private void OnEnable()
	{
		objectType = GridObjectType.PLANT;
	}
}

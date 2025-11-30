using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Plant Object", menuName = "Scriptable Objects/Grid/Grid Plant Data")]
[System.Serializable]
public class GridPlantData : GridObjectData
{
	[Range(0, 3)]
	public int growingStages = 3;
	public int cashReward = 100;
    public int experienceReward = 100;
    public float requiredGrowingTime = 10.0f; // In seconds 
	public int maxHealth = 25; // Max plant health

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

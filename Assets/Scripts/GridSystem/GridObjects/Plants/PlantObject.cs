using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantObject : GridObject
{
    const float MAX_WATER_LEVEL = 100.0f;

	const float MAX_PEST_ATTACK_CHANCE = 100.0f;

    [Header("Plant Settings")]
    [SerializeField] GameObject plantUIPrefab;
	[SerializeField] GridPlantData plantData;
	[SerializeField] int plantHealth = 25;

	// Plant watering
	[Header("Plant Watering Settings")]
    [Range(0f, MAX_WATER_LEVEL)]
    [SerializeField] float plantWaterLevel = MAX_WATER_LEVEL;
    [SerializeField] float plantWaterGainAmount = 2.5f;
    [SerializeField] float plantWaterDrainingAmount = 0.25f;

    // Plant pests
    [Header("Plant Pest Attack Settings")]
    [Range (0f, MAX_PEST_ATTACK_CHANCE)]
    [SerializeField] float pestAttackChance = 25.0f;
	[SerializeField] float minAttackTimer = 10.0f; // In seconds
	[SerializeField] float maxAttackTimer = 20.0f; // In seconds
    [SerializeField] int pestDamage = 1; // Per tick

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip harvestSFX;
    [SerializeField] private AudioClip waterSFX;

    float randomPestAttackTimer = 0.0f; // Gets set in void Start()
    float currentTimeFromLastAttack = 0.0f;

    // Plant growing data
    float currentGrowingTime = 0.0f;
    int currentGrowingStage = 0;

    // A value that tells the plant after how many seconds it should grow
    float timePerGrowingStage = 0.0f; // Gets set in void Start()

    // A value that tracks how much time has passed after it's last growth
    float elapsedTimeSinceLastGrowth = 0.0f;

    MeshFilter plantMeshFilter = null; // The plant's mesh data

	IEnumerator pestAttackCoroutine = null;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		plantMeshFilter = GetComponent<MeshFilter>();
		randomPestAttackTimer = Random.Range(minAttackTimer, maxAttackTimer);

        if (plantData != null)
        {
            // Giving the plant a time frame to "grow" and get bigger
            if (plantData.growingStages > 0)
            {
                timePerGrowingStage = plantData.requiredGrowingTime / plantData.growingStages;
            }
            else
            {
                timePerGrowingStage = plantData.requiredGrowingTime;
            }
        }

		SetGridCellsOccupationalColour();
	}

	// Update is called once per frame
	void Update()
    {
        HandleGrowing();
        DrainWaterLevel();
		HandlePestAttacks();
    }

    // Event that registers when the player has clicked on the plant
	void OnMouseDown()
	{
        if (GameManager.UIManager != null)
        {
            GameManager.UIManager.CreatePlantUI(this, plantUIPrefab);
        }
	}


	void HandleGrowing()
    {
        // Check if the plant still has time to grow
        if (currentGrowingTime < plantData.requiredGrowingTime)
        {
            // Check the time since the last plant growth
            if (elapsedTimeSinceLastGrowth >= timePerGrowingStage)
            {
                ProcessGrowingStage();
                elapsedTimeSinceLastGrowth = 0.0f;
            }

            elapsedTimeSinceLastGrowth += Time.deltaTime;
            currentGrowingTime += Time.deltaTime;
        }
    }

    // Drain the water meter of the plant over time
    void DrainWaterLevel()
    {
        if (IsPlantFullyGrown()) return; // Don't drain water if plant has fully grown

        // Drain the plant's water while it's still in it's growing stages
        float drainAmount = plantWaterDrainingAmount * Time.deltaTime;
        plantWaterLevel -= drainAmount;

        // Plant has run out of water
        if (plantWaterLevel <= 0.0f)
        {
            plantWaterLevel = 0.0f;
            KillPlant();
        }
    }

    void ProcessGrowingStage()
    {
        // Check if plant is supposed to have growing phase
        if (plantData.growingStages > 0)
        {
            // Check if we have set-up meshes the plant needs to grow in
            if (plantData.growingStageMeshes.Length > 0)
            {
                // Check if the next phase of the plant growing mechanic exists 
                if (plantData.growingStageMeshes[currentGrowingStage] != null)
                {
                    Mesh nextMesh = plantData.growingStageMeshes[currentGrowingStage];

                    plantMeshFilter.mesh = nextMesh;

                    currentGrowingStage++;
                }
            }
        }
    }

    void HandlePestAttacks()
    {
        if (pestAttackCoroutine != null) // Ignore the rest of the function if pests are already attacking plant
		{
            if (IsPlantFullyGrown())
            {
                ClearPestDamage(); // Force the pests to stop attacking the plant if it's already fully grown
            }
            return;
        }

        // Check if pests are ready to attack after timer
        if (currentTimeFromLastAttack >= randomPestAttackTimer)
        {
            // Make a small chance for the pests to skip a cycle of attacking the plant and repeat process
            float randomChance = Random.Range(0.0f, MAX_PEST_ATTACK_CHANCE);
            if (randomChance <= pestAttackChance)
            {
                // Create pests that will attack the plant
                pestAttackCoroutine = TakePeriodicPestDamage();
                StartCoroutine(pestAttackCoroutine);
            }


            // Reset the current pest attack timer back to 0, and generate a new wait timer
            currentTimeFromLastAttack = 0.0f;
			randomPestAttackTimer = Random.Range(minAttackTimer, maxAttackTimer);
		}

        currentTimeFromLastAttack += Time.deltaTime;
    }

    IEnumerator TakePeriodicPestDamage()
    {
        while (plantHealth > 0)
        {
            
            plantHealth--;

            yield return new WaitForSeconds(1); // Attack every second
        }

        KillPlant();

        yield return null;
    }

    // Gonna be executed when the "Clear Pests" UI element for the plant is clicked
    public void ClearPestDamage()
    {
        if (pestAttackCoroutine != null)
        {
			StopCoroutine(pestAttackCoroutine);
            pestAttackCoroutine = null;
        }
    }

	public void WaterPlant()
    {
        plantWaterLevel += plantWaterGainAmount;
        if (plantWaterLevel > MAX_WATER_LEVEL)
        {
            plantWaterLevel = MAX_WATER_LEVEL;
        }

        // Play watering sfx
        if (audioSource != null && waterSFX != null)
        {
            audioSource.PlayOneShot(waterSFX);
        }
    }

	public void HarvestPlant()
	{
        // Check if plant has grown long enough to reach the required time
        if (currentGrowingTime >= plantData.requiredGrowingTime)
        {
            Player player = GameManager.player;
            if (player != null)
            {
                player.AddCash(plantData.cashReward);

				// Reset the grid cell's visual object's colour to the default one since the plant is not occupying anything anymore
				ResetGridCellsVisualObjectsColour();

                // Play harvest sfx
                if (audioSource != null && harvestSFX != null)
                {
                    audioSource.PlayOneShot(harvestSFX);
                }

				// Destroy the plant
				KillPlant();
            }
        }
	}

	void KillPlant()
    {
        // If health or water level reaches 0
        if (plantHealth <= 0 || plantWaterLevel <= 0.0f || currentGrowingTime >= plantData.requiredGrowingTime)
        {
            // Reset the colour of the grid cells the plant was previously occupying


            Destroy(gameObject, harvestSFX.length);
        }
    }

	// Change the colour of the grid cell visual objects the plant is staying on top of and any other nearby that it's occupying
	void SetGridCellsOccupationalColour()
    {
        if (GameManager.gridSystem == null || parentCell == null) return;

		List<GameObject> gridCellsVisualObjects = GameManager.gridSystem.FindVisualObjectsBasedOnCell(parentCell, plantData.gridCellRequirement);

		if (gridCellsVisualObjects != null && gridCellsVisualObjects.Count > 0)
		{
			// Loop through all coloured grid cell visual game objects and set them to their default colour
			foreach (GameObject gridCellVisualObject in gridCellsVisualObjects)
			{
				SpriteRenderer spriteRenderer = gridCellVisualObject.GetComponent<SpriteRenderer>();
				if (spriteRenderer != null)
				{
					spriteRenderer.color = Color.red;
				}
			}
		}
	}

    // Same functions as the one on top, but this one just returns back all the cells to the their default colour
    void ResetGridCellsVisualObjectsColour()
    {
		if (GameManager.gridSystem == null || parentCell == null) return;

		List<GameObject> gridCellsVisualObjects = GameManager.gridSystem.FindVisualObjectsBasedOnCell(parentCell, plantData.gridCellRequirement);

		if (gridCellsVisualObjects != null && gridCellsVisualObjects.Count > 0)
		{
			// Loop through all grid cell visual game objects and set them to their default colour
			foreach (GameObject gridCellVisualObject in gridCellsVisualObjects)
			{
				SpriteRenderer spriteRenderer = gridCellVisualObject.GetComponent<SpriteRenderer>();
				if (spriteRenderer != null)
				{
					spriteRenderer.color = GameManager.gridSystem.GetGridCellVisualDefaultColor();
				}
			}
		}
	}

    public float GetPlantGrowingTimeLeft() { return plantData.requiredGrowingTime - currentGrowingTime; }

    public string GetPlantName() { return plantData.objectName; }

    public int GetPlantHealth() {return plantHealth; }
    public float GetPlantWaterLevel() { return plantWaterLevel; }
    public float GetPlantCurrentGrowingTime() { return currentGrowingTime; }

    // Used when the save data file is being loaded (when the player starts the game)
	public void SetPlantHealth(int healthToSet) { plantHealth = healthToSet; }
	public void SetPlantWaterLevel(float waterToSet) { plantWaterLevel = waterToSet; }
	public void SetPlantCurrentGrowingTime(float timeToSet) { currentGrowingTime = timeToSet; }

	public GridPlantData GetPlantData() { return plantData; }

    bool IsPlantFullyGrown() { return (currentGrowingTime >= plantData.requiredGrowingTime); }
}

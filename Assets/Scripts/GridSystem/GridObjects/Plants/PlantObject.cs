using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantObject : GridObject
{
    const float MAX_WATER_LEVEL = 100.0f;

	const float MAX_PEST_ATTACK_CHANCE = 100.0f;
	const float MIN_PEST_ATTACK_TIMER = 10.0f;
    const float MAX_PEST_ATTACK_TIMER = 30.0f;

    [Header("Plant Settings")]
    [SerializeField] GameObject plantUIPrefab;
	[SerializeField] GridPlantData plantData;
	[SerializeField] int plantHealth = 10;

	// Plant watering
	[Header("Plant Watering Settings")]
    [Range(0f, MAX_WATER_LEVEL)]
    [SerializeField] float plantWaterLevel = MAX_WATER_LEVEL;
    [SerializeField] float plantWaterGainAmount = 5.0f;
    [SerializeField] float plantWaterDrainingAmount = 0.25f;

    // Plant pests
    [Header("Plant Pest Attack Settings")]
    [Range (0f, MAX_PEST_ATTACK_CHANCE)]
    [SerializeField] float pestAttackChance = 50.0f;
	[SerializeField] float minAttackTimer = MIN_PEST_ATTACK_TIMER;
	[SerializeField] float maxAttackTimer = MAX_PEST_ATTACK_TIMER;
    [SerializeField] int pestDamage = 1;

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
        else
        {

        }
    }

    // Drain the water meter of the plant over time
    void DrainWaterLevel()
    {
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
        if (pestAttackCoroutine != null) return; // Ignore the rest of the function if pests are already attacking plant

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
    public void StopPeriodicPestDamage()
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
    }

    void KillPlant()
    {
        // If health or water level reaches 0
        if (plantHealth <= 0 || plantWaterLevel <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public int GetPlantHealth() {return plantHealth; }
    public float GetPlantWaterLevel() { return plantWaterLevel; }
    public float GetPlantCurrentGrowingTime() { return currentGrowingTime; }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class PlantObject : GridObject
{
    [SerializeField] GridPlantData plantData;
    
    float currentGrowingTime = 0.0f;

    int currentGrowingStage = 0;

    // A value that tells the plant after how many seconds it should grow
    float timePerGrowingStage = 0.0f;

    // A value that tracks how much time has passed after it's growth
    float elapsedTimeSinceLastGrowth = 0.0f;

    MeshFilter plantMeshFilter = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plantMeshFilter = GetComponent<MeshFilter>();
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
}

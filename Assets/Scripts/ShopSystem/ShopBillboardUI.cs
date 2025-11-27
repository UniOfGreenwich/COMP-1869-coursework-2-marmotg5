using UnityEngine;
using UnityEngine.UIElements;

public class ShopBillboardUI : MonoBehaviour
{
    enum BillboardMovingState
    {
        Up,
        Down
    }

    [Header("UI Elements")]
    [SerializeField] Transform billboardArrow;

    [Header("Wave Settings")]
    [SerializeField, Tooltip("How much it can go down and up on the Y-axis")] 
    float heightAmountWaveLimit = 1.0f;
    [SerializeField] float waveSpeed = 3.0f;

    Vector3 startingPosition = Vector3.zero;
    Vector3 topPosition = Vector3.zero;
    Vector3 bottomPosition = Vector3.zero;

    BillboardMovingState movingState = BillboardMovingState.Up; // Go up first when the game starts

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (billboardArrow != null)
        {
            // Setting the positions for the billboard arrow that will be bobbing/waving
            startingPosition = billboardArrow.localPosition;
            topPosition = startingPosition + new Vector3(0.0f, heightAmountWaveLimit, 0.0f);
            bottomPosition = startingPosition - new Vector3(0.0f, heightAmountWaveLimit, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookTowardsCamera();
        MoveArrow();
    }

    void LookTowardsCamera()
    {
        if (GameManager.mainCamera != null)
        {
            // Force the parent/canvas object to look towards the camera at all times
            transform.LookAt(GameManager.mainCamera.transform);
        }
    }

    void MoveArrow()
    {
        // Make sure we have an arrow object and positions to work with
        if (billboardArrow != null && startingPosition != Vector3.zero)
        {
            // Get the moving direction/state of the billboard arrow
            if (movingState == BillboardMovingState.Up)
            {
                // Move towards the set maxium top position for the arrow
                float distanceToPosition = Vector3.Distance(billboardArrow.transform.localPosition, topPosition);
                if (distanceToPosition <= 0.15f) { movingState = BillboardMovingState.Down; return; } // If the arrow is close to the max limit, switch to the other state

                billboardArrow.localPosition = Vector3.Lerp(billboardArrow.transform.localPosition, topPosition, waveSpeed * Time.deltaTime);

            }
            else if (movingState == BillboardMovingState.Down)
            {
                // Move towards the set maxium bottom position for the arrow
                float distanceToPosition = Vector3.Distance(billboardArrow.transform.localPosition, bottomPosition);
                if (distanceToPosition <= 0.15f) { movingState = BillboardMovingState.Up; return; } // If the arrow is close to the max limit, switch to the other state

                billboardArrow.localPosition = Vector3.Lerp(billboardArrow.transform.localPosition, bottomPosition, waveSpeed * Time.deltaTime);
            }
        }
    }
}

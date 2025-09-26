using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GridSystem : MonoBehaviour
{
    Grid grid;

    int[,] gridArray = { };

    [SerializeField]
    int gridCellWidthAmount = 10;
    [SerializeField]
    int gridCellHeightAmount = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GetComponent<Grid>();
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeGrid()
    {
        for (int y = 0; y < gridCellHeightAmount; y++)
        {
            for (int x = 0; x < gridCellWidthAmount; x++)
            {
            }
        }
    }
}

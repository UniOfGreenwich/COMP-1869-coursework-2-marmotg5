using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class GridCell
{
    (int z, int x) cellIndex; // ex. [0, 1] | 0 = Z , 1 = X
    Vector3 cellPosition;
    float cellSize;

    public GridCell((int z, int x) index, float cellSize, Vector3 cellPosition)
    {
		this.cellIndex = index;
        this.cellPosition = cellPosition;
        this.cellSize = cellSize;

        Debug.Log("grid cell [" + cellIndex.z + ", " + cellIndex.x + "] at pos: " + cellPosition);
	}

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // Checks if there is anything placed on the grid cell block
    public bool IsOccupied()
    {

        return false;
    }

    public Vector3 GetCellCornerPosition() {return cellPosition;}

    public Vector3 GetCenteredPosition()
    {
        return new Vector3((cellPosition.x * cellSize) / 2.0f, cellPosition.y, (cellPosition.z * cellSize) / 2.0f);
    }


}

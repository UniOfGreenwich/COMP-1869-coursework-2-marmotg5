using System;
using Unity.VisualScripting;
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

        //Debug.Log("grid cell [" + cellIndex.z + ", " + cellIndex.x + "] at pos: " + cellPosition);
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

    public bool IsPointInsideGridCell(Vector3 point)
    {
        // Cell vertices
        Vector3 topLeft = cellPosition; // -- cellPosition always starts on the top-left corner of each grid cell
        Vector3 topRight = topLeft + new Vector3(topLeft.x, topLeft.y, topLeft.z + cellSize);
		Vector3 bottomLeft = topLeft + new Vector3(topLeft.x + cellSize, topLeft.y, topLeft.z);
		Vector3 bottomRight = topLeft + new Vector3(topLeft.x + cellSize, topLeft.y, topLeft.z + cellSize);

		// Check if the point is within the grid cell's perimeter
		if (point.x >= topLeft.x &&
			point.x <= bottomLeft.x &&
			//point.y >= bounds.min.y &&
			//point.y <= bounds.max.y &&
			point.z >= topLeft.x &&
			point.z <= topRight.z)
		{
			return true;
		}

		return false;
    }

    public Vector3 GetCellCornerPosition() {return cellPosition;}
    public Vector3 GetCenteredPosition()
    {
        Vector3 halfPosition = new Vector3((cellPosition.x + cellSize) / 2.0f, cellPosition.y, (cellPosition.z + cellSize) / 2.0f);
		return cellPosition + halfPosition;
    }


}

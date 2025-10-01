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

    // Checks if there is anything placed on the grid cell block
    public bool IsOccupied()
    {

        return false;
    }

    public bool IsPointInsideGridCell(Vector3 point)
    {
        // Cell vertices
        Vector3 topLeft = cellPosition; // -- cellPosition always starts on the top-left corner of each grid cell
        Vector3 topRight = new Vector3(topLeft.x, topLeft.y, topLeft.z + cellSize);
		Vector3 bottomLeft = new Vector3(topLeft.x + cellSize, topLeft.y, topLeft.z);
		Vector3 bottomRight = new Vector3(topLeft.x + cellSize, topLeft.y, topLeft.z + cellSize);

        // Giving the cells a bith of depth to make it easier to be clicked on in case the point coordinates are elevated/in air
        float underDepth = cellPosition.y - cellSize;
		float overDepth = cellPosition.y + cellSize;

		// Check if the point is within the grid cell's perimeter
		if (point.x >= topLeft.x &&
            point.x <= bottomLeft.x &&
            point.y >= underDepth &&
            point.y <= overDepth &&
            point.z >= topLeft.z &&
			point.z <= topRight.z)
		{
			return true;
		}

		return false;
    }

    public (int z, int x) GetCellIndex() {return cellIndex;}
    public Vector3 GetCellCornerPosition() {return cellPosition;}
    public Vector3 GetCenteredPosition()
    {
        float halfZ = cellPosition.z + ((cellPosition.z + cellSize) - cellPosition.z) * 0.5f; // Width
		float halfX = cellPosition.x + ((cellPosition.x + cellSize) - cellPosition.x) * 0.5f; // Height
		Vector3 halfPosition = new Vector3(halfX, cellPosition.y, halfZ);
		return halfPosition;
    }


}

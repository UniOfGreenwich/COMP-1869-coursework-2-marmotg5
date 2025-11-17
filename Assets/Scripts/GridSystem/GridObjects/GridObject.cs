using UnityEngine;

public abstract class GridObject : MonoBehaviour
{
    protected GridCell parentCell = null; // The cell the grid object belongs to

	//// Start is called once before the first execution of Update after the MonoBehaviour is created
	//void Start()
	//{

	//}

	//// Update is called once per frame
	//void Update()
	//{

	//}

	protected bool MoveInGrid()
    {

        return false;
    }

    protected bool RemoveFromGrid()
    {

        return false;
    }

    public void UpdateParentCell(GridCell cell)
    {
        if (cell != null) 
            parentCell = cell;
	}

    public GridCell GetObjectParentGridCell() {  return parentCell; }

}

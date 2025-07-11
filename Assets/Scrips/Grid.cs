using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    public Vector3 cellSize = new Vector3(1,1,1);
    public Cell[,] gridObjects;
    public Cell gridCellPrefab;
    public float TotalGridWidth => width*cellSize.x;
    public float TotalGridHeight => height*cellSize.y;
    public void Add(Cell gameObject, int x, int y)
    {
        gridObjects[x, y] = gameObject;
    }

    public void Remove(int x, int y)
    {
        Destroy(gridObjects[x, y].gameObject);
        gridObjects[x, y] = null;
    }

    public bool IsPopulated(int x, int y)
    {
        return gridObjects[x, y] != null;
    }

    public bool CanAdd(int x, int y)
    {
        return gridObjects[x, y] == null;
    }

    public Vector3 GetCellPosition(Vector3Int gridposition)
    {
        return new Vector3(
            gridposition.x * cellSize.x, 
            gridposition.y * cellSize.y, 
            gridposition.z * cellSize.z);
    }

    public void Clear()
    {
        if (gridObjects == null)
        {
            return;
        }
        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int z = 0; z < gridObjects.GetLength(1); z++)
            {
                Remove(x, z);
            }
        }
    }

    public void GenerateGrid()
    {
        Clear();
        gridObjects = new Cell[width, height];
        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int z = 0; z < gridObjects.GetLength(1); z++)
            {
                var newobject = Instantiate(gridCellPrefab);
                newobject.gridPosition = new Vector2Int(x, z);
                newobject.transform.position = GetCellPosition(new Vector3Int(x, 1, z));
                if (CanAdd(x, z))
                {
                    Add(newobject, x, z);
                }
            }
        }
    }

    public void LoadGridFromSave(SavedLevel level)
    {
        width = level.gridSettings.width;
        height = level.gridSettings.height;
        cellSize = level.gridSettings.cellSize.ToVector3();
        GenerateGrid();

        Node lastNode = null;
        foreach (var pathNode in level.gridPath.nodes)
        {
            RecalculatePath(lastNode, pathNode, level.gridPath);
            lastNode = pathNode;
        }
    }

    public void RecalculatePath(Node lastNode, Node newNode, GridPath path){
        var newCell = gridObjects[newNode.gridPosition.x, newNode.gridPosition.y];
        if(lastNode == null){
            newCell.SetNavigationType(CellNavigationType.Start);
            return;
        }

        var lastCell = gridObjects[lastNode.gridPosition.x, lastNode.gridPosition.y];

        if(lastNode.gridPosition == newNode.gridPosition){
            lastCell.SetNavigationType(CellNavigationType.None);

            var newFinishNode = path.nodes[path.nodes.Count - 1];
            var newFinishCell = gridObjects[newFinishNode.gridPosition.x, newFinishNode.gridPosition.y];
            newFinishCell.SetNavigationType(CellNavigationType.Finish);
            return;
        }
        if(lastCell.navigationType != CellNavigationType.Start){
            lastCell.SetNavigationType(CellNavigationType.Path);
        }
        newCell.SetNavigationType(CellNavigationType.Finish);
    }
}
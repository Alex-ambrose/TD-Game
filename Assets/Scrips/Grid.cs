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
        foreach (var pathNode in level.gridPath)
        {
            var cell = gridObjects[pathNode.position.x, pathNode.position.y];
            cell.SetNavigationType(pathNode.cellNavigationType);
        }
    }

    public List<GridPathNode> GetGridPath()
    {
        var gridPath = new List<GridPathNode>();
        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int z = 0; z < gridObjects.GetLength(1); z++)
            {
                var cell = gridObjects[x, z];
                if (cell.HasNavigationType())
                {
                    var pathNode = new GridPathNode();
                    pathNode.position = Vec2Int.FromVector2Int(cell.gridPosition);
                    pathNode.cellNavigationType = cell.navigationType;

                    gridPath.Add(pathNode);
                }
            }
        }
        return gridPath;
    }
}
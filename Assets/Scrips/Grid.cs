using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector3 cellsize;
    private Cell[,] gridObjects;
    public Cell gridcell;

    public int Width => width;
    public int Height => height;
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
        return new Vector3(gridposition.x * cellsize.x, gridposition.y * cellsize.y, gridposition.z * cellsize.z);
    }

    public void Clear()
    {
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
        gridObjects = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newobject = Instantiate(gridcell);

                newobject.transform.position = GetCellPosition(new Vector3Int(x, 1, z));
                if (CanAdd(x, z))
                {
                    Add(newobject, x, z);
                }
            }
        }
    }
    public void GenerateGrid(SavedLevel level)
    {
        gridObjects = new Cell[level.width, level.height];
        for (int x = 0; x < level.width; x++)
        {
            for (int z = 0; z < level.height; z++)
            {
                var newobject = Instantiate(gridcell);

                newobject.transform.position = GetCellPosition(new Vector3Int(x, 1, z));
                if (CanAdd(x, z))
                {
                    Add(newobject, x, z);
                }
                
            }
        }
        foreach(var SpawnedObject in level.SpawnedObjects)
        {
            var cell = gridObjects[SpawnedObject.position.x, SpawnedObject.position.y];
            cell.Toggle();
            cell.setBlocktype(SpawnedObject.blocktype);
        }
    }
    public List<SpawnedObject> GetpopulatedCells()
    {
        var SpawndObjects = new List<SpawnedObject>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (gridObjects[x, z].hasChild())
                {
                    var SpawndedObject = new SpawnedObject();
                    SpawndedObject.position = new Vector2Int(x, z);
                    SpawndedObject.blocktype = gridObjects[x, z].BlockType;

                    SpawndObjects.Add(SpawndedObject);
                }
            }
        }
        return SpawndObjects;
    }
}
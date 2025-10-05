using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedLevel
{
    public GridPath gridPath;
    public GridSettings gridSettings;
}

public class GridPath
{
    public List<Node> nodes;

    public GridPath(){
         nodes = new List<Node>();
    }
}

public class Node
{
    public int pathOrder;
    public Vec2Int gridPosition;
}

public class GridSettings
{
    public int width;
    public int height;
    public Vec3 cellSize;
}

// Custom holder of cell size data to avoid Unity's Vector3 serialization issues
public class Vec3
{
    public float x;
    public float y;
    public float z;

    public static Vec3 FromVector3(Vector3 vector)
    {
        return new Vec3 { x = vector.x, y = vector.y, z = vector.z };
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}


// Custom holder for 2D integer coordinates to avoid Unity's Vector2Int serialization issues
public class Vec2Int
{
    public int x;
    public int y;

    public override int GetHashCode()
    {
        return  x.GetHashCode() + y.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        var CastedObject = obj as Vec2Int;
        return x == CastedObject.x && y == CastedObject.y;
    }

    public static Vec2Int FromVector2Int(Vector2Int vector)
    {
        return new Vec2Int { x = vector.x, y = vector.y };
    }
    public static Vector2Int ToVector2Int(Vec2Int vector)
    {
        
        return new Vector2Int (vector.x, vector.y);
    }
}

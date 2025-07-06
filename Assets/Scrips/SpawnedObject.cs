using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject
{
    public Vector2Int position;
    public int blocktype;

    public override string ToString()
    {
        return $"{position.x}:{position.y}:{blocktype}";
    }
}


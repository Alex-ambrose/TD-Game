using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngineInternal;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public float distance;
    public LayerMask layerMask;
    

    // Start is called before the first frame update
    public void init(string fileName)
    {
        var levelData = File.ReadAllText(fileName);
        var SavedLevel = JsonConvert.DeserializeObject<SavedLevel>(levelData);
        grid.GenerateGrid(SavedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grid.Clear();
            grid.GenerateGrid();
        }
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, distance,layerMask))
            {
                var cell = hit.transform.gameObject.GetComponent<Cell>();
                Debug.Log(hit.transform.gameObject.name);
                Debug.DrawLine(ray.origin, ray.direction * distance,Color.blue, 5);
                if (cell != null)
                {
                    cell.Toggle();
                    
                }
            }
        }

    }


}

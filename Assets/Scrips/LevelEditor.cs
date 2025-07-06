using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{

    public TMP_Text levelName;
    public Button Savebotton;
    public TMP_Dropdown BlockType;
    public int CurrentBlockType;
    public Grid grid;
    public float distance;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        Savebotton.onClick.AddListener(Save);
        BlockType.onValueChanged.AddListener(BlockTypeChanged);
        grid.GenerateGrid();
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

            if (Physics.Raycast(ray, out var hit, distance, layerMask))
            {
                var cell = hit.transform.gameObject.GetComponent<Cell>();
                Debug.Log(hit.transform.gameObject.name);
                Debug.DrawLine(ray.origin, ray.direction * distance, Color.blue, 5);
                if (cell != null)
                {
                    cell.Toggle();
                    cell.setBlocktype(CurrentBlockType);
                }
            }
        }
    }
    void Save()
    {
        if(string.IsNullOrEmpty (levelName.text))
        {
            Debug.LogError("You tried to save a level without a name");
            return; 
          
        
        }
        //get a list of all spawned objects (position, type)
        var SpawnedObjects = grid.GetpopulatedCells();
        var SavedLevel = new SavedLevel();
        SavedLevel.SpawnedObjects = SpawnedObjects;
        SavedLevel.width = grid.Width;
        SavedLevel.height = grid.Height;
        //write it to a file
        var fileName = Path.Combine(Application.persistentDataPath, levelName.text);
        var fileData = JsonConvert.SerializeObject(SavedLevel);
        File.WriteAllText(fileName, fileData);
    }

    void BlockTypeChanged(int value)
    {
        CurrentBlockType = value;
    }
}

using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public float distance;
    public LayerMask layerMask;

    public void LoadFromSave(string fileName)
    {
        var levelData = File.ReadAllText(fileName);
        var SavedLevel = JsonConvert.DeserializeObject<SavedLevel>(levelData);
        grid.LoadGridFromSave(SavedLevel);
    }

    void Update()
    {
        if(TryClickCell(out Cell cell))
        {
            cell.Toggle();
        }
    }

    private bool TryClickCell(out Cell cell)
    {
        cell = null;

        if (!Input.GetMouseButtonDown(0))
        {
            return false;
        }

        var mousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, distance, layerMask))
        {
            cell = hit.transform.gameObject.GetComponent<Cell>();
            Debug.Log(hit.transform.gameObject.name);
            Debug.DrawLine(ray.origin, ray.direction * distance, Color.blue, 5);
        }

        return cell != null;
    }
}

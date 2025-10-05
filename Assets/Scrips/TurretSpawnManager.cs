using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class TurretSpawnManager : MonoBehaviour
{
    public Turret CurrentTurret;
    public float distance;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TryClickCell(out Cell cell))
        {
            Spawn(cell);
        }
    }
    public void Spawn(Cell cell)
    {

        var newturret = Instantiate(CurrentTurret.prefab);
        var newPosition = Vec2Int.FromVector2Int(cell.gridPosition);
        newturret.transform.position = GameManager.Instance.grid.GetPlaceableWorldPosition(newPosition);

        
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

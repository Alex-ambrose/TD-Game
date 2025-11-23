using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnManager : Singleton<TurretSpawnManager>
{
    public TurretSO SelectedTurretPrefab;
    TurretController SelectedTurretInstance;
    MeshRenderer SelectedTurretRenderer;
    public float distance;
    public LayerMask layerMask;
    public Dictionary<string, TurretController> SpawnedTurrets;
    public static readonly int TurretLayer = 6;
    // Start is called before the first frame update
    void Start()
    {
        SpawnedTurrets = new Dictionary<string, TurretController> ();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(SelectedTurretInstance.gameObject);
            SelectedTurretPrefab = null;
            SelectedTurretInstance = null;
            SelectedTurretRenderer = null;
            return;
        }
        if (SelectedTurretPrefab == null)
        {
            return;
        }
        Cell CurrentCell = GetHoveredCell();
        if (CurrentCell == null || CurrentCell.HasNavigationType())
        {
            SelectedTurretInstance.gameObject.SetActive(false);
            return; 
        }
        var newPosition = Vec2Int.FromVector2Int(CurrentCell.gridPosition);
        SelectedTurretInstance.transform.position = GameManager.Instance.grid.GetPlaceableWorldPosition(newPosition);
        SelectedTurretInstance.gameObject.SetActive(true);
        if (Input.GetMouseButtonDown(0))
        {
           Spawn(newPosition);
        }
        

    }
    // If we dont have a selected turret => do nothing
    // IF we have a selected turret => check if were hovering over a block move my turret to that block
    // IF we click => 
    // If we are on a valid Cell => spend money and fix prefab material and clear selected turret prefab
    // If we dont click a valid Cell => clear selected turret prefab
    public void Spawn(Vec2Int newPosition)
    {
        if (SpawnedTurrets.ContainsKey($"{newPosition.x},{newPosition.y}"))
        {
            return;
        }
        if (GameManager.Instance.gameState.Gold < SelectedTurretPrefab.Cost)
        {
            return;
        }
        
        GameManager.Instance.gameState.Gold -= SelectedTurretPrefab.Cost;

        SpawnedTurrets.Add($"{newPosition.x},{newPosition.y}", SelectedTurretInstance);
        var transparentColor = SelectedTurretRenderer.material.color;
        transparentColor.a = 1f;
        SelectedTurretRenderer.material.color = transparentColor;
        SelectedTurretPrefab = null;
        SelectedTurretInstance = null;
        SelectedTurretRenderer = null;

        
    }

    private Cell GetHoveredCell()
    {
        Cell cell = null;

        var mousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, distance, layerMask))
        {
            cell = hit.transform.gameObject.GetComponent<Cell>();
            Debug.Log(hit.transform.gameObject.name);
            Debug.DrawLine(ray.origin, ray.direction * distance, Color.blue, 5);
        }

        return cell;
    }

    public void SetCurrentTurret(TurretSO turretSO)
    {
        SelectedTurretPrefab = turretSO;
        SelectedTurretInstance = Instantiate(SelectedTurretPrefab.prefab);
        SelectedTurretInstance.TurretStats = new TurretStats(SelectedTurretPrefab);
        SelectedTurretInstance.gameObject.layer = TurretLayer;
        SelectedTurretInstance.gameObject.SetActive(false);
        SelectedTurretRenderer = SelectedTurretInstance.GetComponent<MeshRenderer>();
        var transparentColor = SelectedTurretRenderer.material.color;
        transparentColor.a = 0.7f;
        SelectedTurretRenderer.material.color = transparentColor;
    }

    public void ClearCurrentTurret()
    {
        SelectedTurretPrefab = null;
        SelectedTurretInstance = null;
        SelectedTurretRenderer = null;
    }
}

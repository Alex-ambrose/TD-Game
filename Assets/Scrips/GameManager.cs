using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Grid grid;
    public float distance;
    public LayerMask layerMask;

    public List<Wave> waves;
    public List<Enemy> enemies;
    
    public float spawnInterval;
    private WaveState waveState = new WaveState();

    public SavedLevel currentLevel;
    private List<EnemyController> spawnedEnemies = new List<EnemyController>();
    

    public void LoadFromSave(string fileName)
    {
        var levelData = File.ReadAllText(fileName);
        currentLevel = JsonConvert.DeserializeObject<SavedLevel>(levelData);
        grid.LoadGridFromSave(currentLevel);
    }


    void Update()
    {
        if (TryClickCell(out Cell cell))
        {
            cell.Toggle();
        }

        if (waveState.status == WaveStatus.Spawning)
        {
            waveState.spawnTimer += Time.deltaTime;
            if (waveState.spawnTimer > spawnInterval)
            {
                SpawnEnemy();
                waveState.spawnTimer = 0;
            }
        }

        if(waveState.status == WaveStatus.Running && spawnedEnemies.Count == 0)
        {
            CompleteWave();
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

    public void SpawnNextWave()
    {
        waveState.status = WaveStatus.Spawning;
        waveState.pointsRemaining = waves[waveState.currentWaveIndex].pointValue;
    }

    private void SpawnEnemy()
    {
        var currentWave = waves[waveState.currentWaveIndex];

        var availableEnemies = enemies.Where(e => e.cost <= waveState.pointsRemaining && e.availableAtWave >= waveState.currentWaveIndex).ToList();
        var randomIndex = Random.Range(0, availableEnemies.Count());
        var enemy = availableEnemies[randomIndex];

        if (enemy.cost <= waveState.pointsRemaining)
        {
            waveState.pointsRemaining -= enemy.cost;
            Debug.Log("Spawn Enemy");
            var newEnemy = Instantiate(enemy.prefab);
            spawnedEnemies.Add(newEnemy);
        }

        if (waveState.pointsRemaining == 0)
        {
            waveState.status = WaveStatus.Running;
        }
    }

    // Called when enemy reached the end, or killed by tower
    public void KillEnemy(EnemyController enemy)
    {
        spawnedEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void CompleteWave()
    {
        waveState.status = WaveStatus.Completed;
    }
}


public class WaveState
{
    private WaveStatus _status = WaveStatus.Completed;
    public WaveStatus status
    {
        get 
        {
            return _status;
        }
        set
        {
            _status = value;
            HudController.Instance.OnWaveStateChanged(_status);
        }
    }

    public float spawnTimer;
    public int pointsRemaining;
    public int currentWaveIndex = 0;
}

public enum WaveStatus
{
    Spawning,
    Running,
    Completed
}
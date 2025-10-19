using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("Drag & Drop from scene")]
    public Grid grid;
    public EnemyController enemyPrefab;
    [Header("Drag & Drop from Assets")]
    public List<Enemy> enemies;
    [Header("Spawning Information")]
    public float spawnInterval;
    [SerializeField]
    public GameState gameState;
    public SavedLevel currentLevel;
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    public const string levelFolderName = "Levels";
    public const string GameStateFolderName = "GameState";
    public const string fileExtention = ".json";

    

    public void LoadFromSave(string fileName)
    {
        
        var levelDataFolderPath = Path.Combine(Application.persistentDataPath, levelFolderName);
        var levelDataFilePath = Path.Combine(levelDataFolderPath, fileName);
        var levelData = File.ReadAllText(levelDataFilePath);
        var GameStateFolderPath = Path.Combine(Application.persistentDataPath, GameStateFolderName);
        var GameStateFilePath = Path.Combine(GameStateFolderPath, fileName);
        var GameState = File.ReadAllText(GameStateFilePath);

        currentLevel = JsonConvert.DeserializeObject<SavedLevel>(levelData);
        gameState = JsonConvert.DeserializeObject<GameState>(GameState);
        if (gameState.currentWaveIndex == 0)
        {
            gameState.Gold = 50;
            gameState.lives = 10;
        }
        gameState.status = WaveStatus.Completed;
        grid.LoadGridFromSave(currentLevel);
    }


    void Update()
    {
        

        if (gameState.status == WaveStatus.Spawning)
        {
            gameState.spawnTimer += Time.deltaTime;
            if (gameState.spawnTimer > spawnInterval)
            {
                SpawnEnemy();
                gameState.spawnTimer = 0;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }
    public void SpawnNextWave()
    {
        gameState.status = WaveStatus.Spawning;
        gameState.pointsRemaining = (gameState.currentWaveIndex *5)+5;
    }

    private void SpawnEnemy()
    {

        var availableEnemies = enemies.Where(e => e.cost <= gameState.pointsRemaining && e.availableAtWave <= gameState.currentWaveIndex).ToList();
        var randomIndex = Random.Range(0, availableEnemies.Count());
        var enemy = availableEnemies[randomIndex];

        if (enemy.cost <= gameState.pointsRemaining)
        {
            gameState.pointsRemaining -= enemy.cost;
            Debug.Log("Spawn Enemy");
            var newEnemy = Instantiate(enemyPrefab);
            spawnedEnemies.Add(newEnemy);
            newEnemy.Setup(enemy);
        }

        if (gameState.pointsRemaining == 0)
        {
            gameState.status = WaveStatus.Running;
        }
    }

    // Called when enemy reached the end, or killed by tower
    public void KillEnemy(EnemyController enemy,bool isFinished)
    {
        if (isFinished)
        {
            --gameState.lives;
            HudController.Instance.UpdateLives(gameState.lives);
        }
        else
        {
            gameState.Gold += enemy.Enemy.cost * 10;
        }
        spawnedEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (gameState.status == WaveStatus.Running && spawnedEnemies.Count == 0)
        {
            CompleteWave();
        }
    }

    public void CompleteWave()
    {
        gameState.currentWaveIndex++;
        gameState.status = WaveStatus.Completed;
        SaveGameState();
    }

    private void SaveGameState()
    {
        var GameStateFolderPath = Path.Combine(Application.persistentDataPath, GameStateFolderName);
        var GameStateFilePath = Path.Combine(GameStateFolderPath, currentLevel.levelName);
        if (!Directory.Exists(GameStateFolderPath))
        {
            Directory.CreateDirectory(GameStateFolderPath);
        }
        var savedData = JsonConvert.SerializeObject(gameState);
        File.WriteAllText(GameStateFilePath + fileExtention, savedData);
    }
}


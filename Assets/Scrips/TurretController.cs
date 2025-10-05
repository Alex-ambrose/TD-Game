using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public ProjectileController projectilePrefab;
    public Transform ProjectileSpawnLocation;
    public enum TargetingStatus
    {
        closest, furthest,strongest,weakest
    }
    public TargetingStatus targeting;
    public Turret Stats;
    public float ShotTimer;
    public float interval => 1/Stats.AttackSpeed;
    void Start()
    {
        
    }

    // TODO make turret follow target
    // TODO
    void Update()
    {
        ShotTimer += Time.deltaTime;
        
        if (interval < ShotTimer)
        {
            var targetenemy = GetTarget();
            if (targetenemy != null)
            {
                Shoot(targetenemy);
            }
            ShotTimer = 0;
        }
    }
    private EnemyController GetTarget()
    {
        var gameManager = GameManager.Instance;
        // prevents an error where turret can't find a targer because there are no enemys Edge Case
        if (gameManager.spawnedEnemies == null || gameManager.spawnedEnemies.Count == 0)
        {
            return null;
        }
        var gridPath = gameManager.currentLevel.gridPath;
        switch (targeting)
        {
            case TargetingStatus.closest:
                return gameManager.spawnedEnemies.OrderByDescending(e => e.currentPathIndex).First();
            case TargetingStatus.furthest:
                return gameManager.spawnedEnemies.OrderBy(e => e.currentPathIndex).First();
            case TargetingStatus.strongest:
                return gameManager.spawnedEnemies.OrderByDescending(e => e.Enemy.stats.currentHealth).First();
            case TargetingStatus.weakest:
                return gameManager.spawnedEnemies.OrderBy(e => e.Enemy.stats.currentHealth).First();
            default:
                return null;
        }
    }
    private void Shoot(EnemyController enemy)
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.SetTarget(enemy, Stats);
        projectile.transform.position = ProjectileSpawnLocation.position;

    }
}

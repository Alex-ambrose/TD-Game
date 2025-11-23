using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum TargetingStatus
{
    closest, furthest, strongest, weakest
}

public class TurretController : MonoBehaviour
{
    [Header("Set in Editor")]
    public ProjectileController projectilePrefab;
    public Transform ProjectileSpawnLocation;
    public Transform TurretTop;
    public float RotationSpeed;

    [Header("Set at Runtime")]
    public TargetingStatus targeting;
    public TurretStats TurretStats;
    public float ShotTimer;

    void Start()
    {
        
    }

    void Update()
    {
        var targetenemy = GetTarget();
        ShotTimer += Time.deltaTime;
        if (targetenemy == null)   
        {
            return;
        }

        LookAtTarget(targetenemy.transform);

        if (ShotTimer > TurretStats.AttackInterval)
        {
            Shoot(targetenemy);
            
            ShotTimer = 0;
        }
    }

    private void LookAtTarget(Transform target)
    {
        var direction = (TurretTop.position - target.position).normalized;
        var rotation = Quaternion.LookRotation(direction);
        var rotationAngles = rotation.eulerAngles;
        rotationAngles.x = 0;
        rotationAngles.z = 0;
        rotation = Quaternion.Euler(rotationAngles);
        TurretTop.rotation = Quaternion.RotateTowards(TurretTop.rotation, rotation, Time.deltaTime * RotationSpeed);
    }

    private EnemyController GetTarget()
    {
        var gameManager = GameManager.Instance;
        // prevents an error where turret can't find a targer because there are no enemys Edge Case
        if (gameManager.spawnedEnemies == null || gameManager.spawnedEnemies.Count == 0)
        {
            return null;
        }

        var enemiesInRange = gameManager.spawnedEnemies.Where(e => Vector3.Distance(e.transform.position, transform.position) <= TurretStats.Range).ToList();
        var gridPath = gameManager.currentLevel.gridPath;
        switch (targeting)
        {
            case TargetingStatus.closest:
                return enemiesInRange.OrderByDescending(e => e.currentPathIndex).First();
            case TargetingStatus.furthest:
                return enemiesInRange.OrderBy(e => e.currentPathIndex).First();
            case TargetingStatus.strongest:
                return enemiesInRange.OrderByDescending(e => e.Enemy.stats.currentHealth).First();
            case TargetingStatus.weakest:
                return enemiesInRange.OrderBy(e => e.Enemy.stats.currentHealth).First();
            default:
                return null;
        }
    }

    private void Shoot(EnemyController enemy)
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.SetTarget(enemy, TurretStats);
        projectile.transform.position = ProjectileSpawnLocation.position;
    }
}

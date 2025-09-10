using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStats stats;
    private Collider myCollider;

    private int currentPathIndex = 0;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        stats.currentHealth = stats.maxHealth;
        transform.position = GameManager.Instance.grid.GetPlaceableWorldPosition(GameManager.Instance.currentLevel.gridPath.nodes[0].gridPosition);
        transform.position += new Vector3(0, GetMyHeight(), 0);
    }


    // TODO: walk to end
    // take damage from towers
    // die and notify game manager
    void Update()
    {
        var nextNode = GameManager.Instance.currentLevel.gridPath.nodes[currentPathIndex + 1];
        var nextPosition = GameManager.Instance.grid.GetPlaceableWorldPosition(nextNode.gridPosition);
        nextPosition += new Vector3(0, GetMyHeight(), 0);
        var movementDesired = nextPosition - transform.position;

        if (movementDesired.magnitude < 0.1f)
        {
            currentPathIndex++;
            if(currentPathIndex + 1 >= GameManager.Instance.currentLevel.gridPath.nodes.Count)
            {
                GameManager.Instance.KillEnemy(this);
                return;
            }
            
            nextNode = GameManager.Instance.currentLevel.gridPath.nodes[currentPathIndex];
            nextPosition = GameManager.Instance.grid.GetPlaceableWorldPosition(nextNode.gridPosition);
            nextPosition += new Vector3(0, GetMyHeight(), 0);
            movementDesired = nextPosition - transform.position;
        }

        transform.position += movementDesired * Time.deltaTime * stats.speed;
    }

    private float GetMyHeight()
    {
        return myCollider.bounds.extents.y;
    }
}

[Serializable]
public class EnemyStats
{
    public int maxHealth;
    public int currentHealth;
    public float speed;
}

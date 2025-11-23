using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class EnemyController : MonoBehaviour
{

    public TMP_Text NameText;
    private Collider myCollider;
    private MeshRenderer myRenderer;
    public int currentPathIndex = 0;
    public Enemy Enemy;
    private Color myColor=> Enemy.Color;
    public EnemyStats stats=> Enemy.stats;
    private float TimeSinceHealed;
    private const float HealInterval = 1;

    public void Setup(Enemy enemy)
    {
        Enemy = enemy;

        NameText.text = enemy.name;
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = myColor;
        stats.currentHealth = stats.maxHealth;
        transform.position = GameManager.Instance.grid.GetPlaceableWorldPosition(GameManager.Instance.currentLevel.gridPath.nodes[0].gridPosition);
        transform.position += new Vector3(0, GetMyHeight(), 0);
    }


    // TODO: walk to end
    // take damage from towers
    // die and notify game manager
    void Update()
    {
        TryMovement();
        TryHeal();
        
    }

    public void UpdateHealth(float healthChange)
    {
        
        stats.currentHealth += healthChange;
        var newColor = myRenderer.material.color;
        newColor.a = Mathf.Lerp(0,1,stats.currentHealth/stats.maxHealth);
        myRenderer.material.color = newColor;
        if (stats.currentHealth <= 0)
        {
            GameManager.Instance.KillEnemy(this,false);
        }
    }

    private float GetMyHeight()
    {
        return myCollider.bounds.extents.y;
    }

    public void TryMovement()
    {
        var nextNode = GameManager.Instance.currentLevel.gridPath.nodes[currentPathIndex + 1];
        var nextPosition = GameManager.Instance.grid.GetPlaceableWorldPosition(nextNode.gridPosition);
        nextPosition += new Vector3(0, GetMyHeight(), 0);
        var movementDesired = nextPosition - transform.position;

        if (movementDesired.magnitude < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex + 1 >= GameManager.Instance.currentLevel.gridPath.nodes.Count)
            {
                GameManager.Instance.KillEnemy(this, true);
                return;
            }

            nextNode = GameManager.Instance.currentLevel.gridPath.nodes[currentPathIndex];
            nextPosition = GameManager.Instance.grid.GetPlaceableWorldPosition(nextNode.gridPosition);
            nextPosition += new Vector3(0, GetMyHeight(), 0);
            movementDesired = nextPosition - transform.position;
        }

        transform.position += movementDesired.normalized * Time.deltaTime * stats.speed;
    }
    
    public void TryHeal()
    {
        TimeSinceHealed += Time.deltaTime;
        if(TimeSinceHealed > HealInterval)
        {
            UpdateHealth(stats.healthPerSecond);
            TimeSinceHealed = 0;
        }
    }
}




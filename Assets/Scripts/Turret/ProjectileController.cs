using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public EnemyController myTarget;
    public float damage;

    void Update()
    {
        if (myTarget == null)
        {
            Destroy(gameObject);
            return;
        }
        var movementDesired = myTarget.transform.position - transform.position;
        if (movementDesired.magnitude < 0.1f)
        {
            HitEnemy();

        }
        transform.position += movementDesired.normalized * Time.deltaTime * 15;
    }
    public void SetTarget(EnemyController enemy, float damage)
    {
        myTarget = enemy;
        this.damage = damage;
    }

    public void HitEnemy()
    {
        myTarget.UpdateHealth(-damage);
        Destroy(gameObject);
    }

}


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public EnemyController myTarget;
    //todo move me towards the target 
    //todo onclolide deal damage
    void Update()
    {
        var movementDesired = myTarget.transform.position - transform.position;
        if (movementDesired.magnitude < 0.1f)
        {
            movementDesired = myTarget.transform.position - transform.position;

        }
        transform.position += movementDesired.normalized * Time.deltaTime * 15;
    }
    public void SetTarget(EnemyController enemy)
    {
        myTarget = enemy;
    }
}


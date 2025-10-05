using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    
    
    //todo move me towards the target 
    //todo onclolide deal damage
    void Update()
    {
        
    }
    public void SetTarget(EnemyController enemy)
    {
        var movementDesired = enemy.transform.position - transform.position;
        if (movementDesired.magnitude < 0.1f)
        {   
            movementDesired = enemy.transform.position - transform.position;
        }
        transform.position += movementDesired.normalized * Time.deltaTime * 15;
    }
}


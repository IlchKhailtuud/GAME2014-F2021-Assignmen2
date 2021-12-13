using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        //if animator not playing idle animation 
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animState = 1;
            enemy.MoveToTarget();
        }
        
        // if enemy reched target point 
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.TransitionToState(enemy.patrolState);
        }
        
        //if enemy does not have a target 
        if (enemy.attackList.Count > 0)
            enemy.TransitionToState(enemy.attackState);
    }
}

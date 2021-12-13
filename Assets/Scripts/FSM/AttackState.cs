using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        //if enemy does not have a target 
        if (enemy.attackList.Count == 0)
            enemy.TransitionToState(enemy.patrolState);
        
        //if enemy has more than 1 target
        if (enemy.attackList.Count > 1)
        {
            //go tgrough all the target and compare distance
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) <
                    Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    //choose the target that has the shortest distance with player
                    enemy.targetPoint = enemy.attackList[i];
                }
            }
        }
        
        //if enemy attack has only one target
        if (enemy.attackList.Count == 1)
            //set the first one in the list as target
            enemy.targetPoint = enemy.attackList[0];

        if (enemy.targetPoint.CompareTag("Player"))
            enemy.AttackAction();
        if (enemy.targetPoint.CompareTag("Bomb"))
            enemy.SkillAction();

        enemy.MoveToTarget();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyBaseState currentState;
    
    public Animator anim;
    public int animState;

    private GameObject alarmSign;
    
    [Header("Base State")] 
    public float health;
    public bool isDead;
    
    [Header("Movement")] 
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    private float nextAttack = 0;
    public float attackRange, skillRange;
    
    public List<Transform> attackList = new List<Transform>();
    
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
        
        GameManager.instance.IsEnemy(this);
    }

    void Start()
    {
        Init();
        TransitionToState(patrolState);
    }
    
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();
    }

    public void AttackAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }
    
    public virtual void SkillAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                anim.SetTrigger("skill");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    //flip avatar by rotating along y-axis
    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else 
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) >
            Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    //once gameobjects enters enemy's detection range, add to target list
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!attackList.Contains(other.transform) && !isDead && !GameManager.instance.gameOver)
            attackList.Add(other.transform);
    }

    //once gameobjects leaves enemy's detection range, remove it from target list 
    private void OnTriggerExit2D(Collider2D other)
    {
        attackList.Remove(other.transform);
    }

    // when gameobjects enter enemy's detection range, play alarm animation
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && !GameManager.instance.gameOver)
            StartCoroutine(OnALarm());
    }

    //coroutine for playing alarm animation
    IEnumerator OnALarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}

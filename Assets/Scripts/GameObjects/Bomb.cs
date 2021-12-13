using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;
    private Rigidbody2D rb;
    
    public float startTime;
    public float waitTime;
    public float bombForce;
    
    [Header("Check")] 
    public float radius;
    public LayerMask tragetLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    private void Update()
    {
        // if animator not playing bomb_off, then play explosion 
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("bomb_explosion");
            }
        }
    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position, radius);
    // }

    public void Explosion()
    {
        col.enabled = false;
        //get all the gameobjects around the bomb
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, tragetLayer);
        
        rb.gravityScale = 0;
        
        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * bombForce, ForceMode2D.Impulse);

            //if explosion makes contact with other bombs, then turn them on again
            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            
            //if explosion makes contact with the player, then call gethit function
            if (item.CompareTag("Player"))
                item.GetComponent<IDamageable>().GetHit(1);
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
    
    //blow off bomb
    public void TurnOff()
    {
        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    //ignite bomb
    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}

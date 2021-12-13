using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;
    
    public float speed;
    public float jumpForce;

    [Header("Player State")] 
    public float health;
    public bool isDead;
    
    [Header("GroundCheck")] 
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    
    [Header("States Check")] 
    public bool isGround;
    public bool isJump;
    public bool canJump;

    [Header("Attack Settings")] 
    public GameObject bombPre;
    public float nextAttack = 0;
    public float attackRate;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();
    }
    
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;
        CheckInput();
        
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        PhysicsCheck();
        Movement();
        Jump();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Movement()
    {
        //keyboard movement
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        //touch input
        float horizontalInput = joystick.Horizontal;
        
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // if (horizontalInput != 0)
        // {
        //     transform.localScale = new Vector3(horizontalInput, 1, 1);
        // }

        //changed flipping method to prevent decimal points
        if (horizontalInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    
    public void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;
            isJump = true;
            canJump = false;
        }
    }

    public void ButtonJump()
    {
        canJump = true;
    }

    //place bomb
    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPre, transform.position, bombPre.transform.rotation);
            nextAttack = Time.time + attackRate;
        }
    }

    public void AddHealth()
    {
        if (health >= 3)
            return;
        else
        {
            health += 1;
        }
        
        UIManager.instance.UpdateHealth(health);
    }

    //check if player is on the ground
    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");
            UIManager.instance.UpdateHealth(health);
        }
    }
}

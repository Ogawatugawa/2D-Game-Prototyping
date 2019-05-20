using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    [Header("Component Set Up")]
    private CharacterController2D charC;
    private Rigidbody2D rigid;

    [Header("Movement Variables")]
    public float baseSpeed = 5f;
    public float moveSpeed;
    public Vector2 motion;
    public Vector2 direction = new Vector2(0,-1);

    [Header("Dash Variables")]
    public float dashSpeed;
    public float dashTimer, maxDashTime, cooldownTimer, dashCooldown;
    private bool isDashing;

    [Header("Attack Variables")]
    public float attackDamage= 20f;
    public float attackRadius = 0.8f;
    public float attackRange = 1.2f;
    public bool CanAttack = true;
    public bool CanBeDamaged = true;

    [Header("Animation")]
    public Animator anim;
    public SpriteRenderer rend;
    
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        charC = GetComponent<CharacterController2D>();
        dashTimer = maxDashTime;
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        motion = new Vector2(inputH, inputV);

        // If we are currently sensing any input
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // Set our Direction variable as Motion (i.e. the last direction we travelled in based on inputs)
            direction = motion.normalized;
        }

        Debug.DrawRay(transform.position, direction);

        // Multiply Motion by Move Speed
        motion.x *= moveSpeed;
        motion.y *= moveSpeed;

        // Run Dash() which will Dash if Left Shift is pressed
        Dash();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        Animate();

        // Move using Character Controller function
       charC.Move(motion * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)direction * attackRange, attackRadius);
    }

    void Attack()
    {
        if (!isDashing)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + direction, attackRadius);
            //Physics.OverlapBox(transform.position + (Vector3)direction, Vector3.one * .25f + Vector3.right,Quaternion.Euler((Vector3)direction));
            foreach (var collider in hitColliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy)
                {
                    print("Enemy hit");
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

    void Dash()
    {
        // If Dash is off cooldown
        if (cooldownTimer > dashCooldown)
        {
            // And we press Left Shift
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                // Set our dash timer to 0
                dashTimer = 0;
            }
        }

        // Else Dash is currently on cooldown
        else
        {
            // So count up the cooldown timer
            cooldownTimer += Time.deltaTime;
        }

        // If Dash is active, i.e. our dash timer is on and counting up
        if (dashTimer < maxDashTime)
        {
            // Motion becomes our last faced direction multiplied by our dash speed
            motion = direction * dashSpeed;
            // Count up the dash timer by Time.deltaTime
            dashTimer += Time.deltaTime;
            // Set IsDashing to true
            isDashing = true;
            // Keep the cooldown timer to 0 so Dash doesn't start cooling down until the Dash is completed
            cooldownTimer = 0;
        }

        else
        {
            isDashing = false;
        }
    }

    void Animate()
    {
        if (!(direction.x == 0 && direction.y == 0))
        {
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);
            anim.SetFloat("Motion", motion.magnitude);
            if (direction.x < 0)
            {
                rend.flipX = true;
            }
            if (direction.x > 0)
            {
                rend.flipX = false;
            } 
        }
    }
}

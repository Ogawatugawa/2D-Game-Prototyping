using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float baseSpeed = 5f, moveSpeed;
    public CharacterController2D charC;
    Rigidbody2D rigid;

    public Vector3 motion;
    public Vector2 direction;

    public float dashSpeed, dashTimer, maxDashTime, cooldownTimer, dashCooldown;

    private bool isDashing;


    // Start is called before the first frame update
    void Start()
    {
        charC = GetComponent<CharacterController2D>();
        dashTimer = maxDashTime;
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        motion = new Vector3(inputH, inputV, 0);

        // If we are currently sensing any input
        if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0)
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

        // Move using Character Controller function
        charC.Move(motion * Time.deltaTime);
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
            motion = direction*dashSpeed;

            // Count up the dash timer by Time.deltaTime
            dashTimer += Time.deltaTime;
            // Keep the cooldown timer to 0 so Dash doesn't start cooling down until the Dash is completed
            cooldownTimer = 0;
        }
    }
}

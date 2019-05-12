using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float baseSpeed = 5f, moveSpeed;
    public CharacterController2D charC;

    public Vector3 motion;

    public Vector2 faceDir;
    public int horizontalDir;
    public int verticalDir;

    public float dashSpeed, dashTimer, maxDashTime, cooldownTimer, dashCooldown;

    private bool isDashing;


    // Start is called before the first frame update
    void Start()
    {
        charC = GetComponent<CharacterController2D>();
        dashTimer = maxDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        motion = new Vector3(inputH, inputV, 0);
        if (inputH > 0)
        {
            horizontalDir = 1;
        }

        if (inputH < 0)
        {
            horizontalDir = -1;
        }

        if (inputV > 0)
        {
            verticalDir = 1;
        }

        if (inputV < 0)
        {
            verticalDir = -1;
        }

        if (inputH != 0 && inputV == 0)
        {
            verticalDir = 0;
        }

        if (inputV != 0 && inputH == 0)
        {
            horizontalDir = 0;
        }

        motion.x *= moveSpeed;
        motion.y *= moveSpeed;
        Dash();
        charC.Move(motion * Time.deltaTime);
    }

    void Dash()
    {
        if (cooldownTimer > dashCooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashTimer = 0;
            }
        }

        else
        {
            cooldownTimer += Time.deltaTime;
        }

        if (dashTimer < maxDashTime)
        {
            motion.x = dashSpeed;
            motion.y = dashSpeed;
            motion.x *= horizontalDir;
            motion.y *= verticalDir;
            dashTimer += Time.deltaTime;
            cooldownTimer = 0;
        }
    }

    //IEnumerator DashRoutine(float startDash, float delay)
    //{
    //    motion.x *= startDash;
    //    motion.y *= startDash;
    //    yield return new WaitForSeconds(delay);
    //}
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementForce = 7F;
    public float jumpForce = 2.3F;
    public float jumpCoolDown;
    public float jumpCounterResetTime;
    public float sprintForce;
    public bool isGrounded = false;


    [SerializeField]
    private float groundCheckRayLength = 0.02F;
    private int jumpsInARow = 4;
    private float jumpCompleteReset;
    private int numberOfJumps = 0;
    private float nextJumpTime;
    private bool spacePressed = false;
    private bool lShiftPressed = false;
    [SerializeField]
    private RaycastHit groundCheck;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        spacePressed = Input.GetKeyDown(KeyCode.Space);
        lShiftPressed = Input.GetKey(KeyCode.LeftShift);

    }


    private void FixedUpdate()
    {
        walking();
        sprint();
        jump();
    }

    private void walking()
    {
        Vector2 playerMovement = new Vector2();
        playerMovement.x = Input.GetAxis("Horizontal");
        playerMovement.y = Input.GetAxis("Vertical");
        playerMovement.Normalize();

        rb.AddForce(playerMovement.x * movementForce, rb.velocity.y, playerMovement.y * movementForce, ForceMode.Force);
    }


    private void jump()
    {
        
        if(Physics.Raycast(transform.position, -Vector3.up, out groundCheck, groundCheckRayLength, LayerMask.GetMask("Ground")))
        {
            Debug.DrawRay(transform.position, -Vector3.up * groundCheck.distance, Color.green);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if(Time.time > nextJumpTime) //if the current time in unity is greater than the nextJumpTime then the player can jump
        {
            if(spacePressed && isGrounded && numberOfJumps <= jumpsInARow)
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                numberOfJumps++;
                jumpCompleteReset = Time.time + jumpCounterResetTime;
                spacePressed = false;
                Debug.Log("Jumping");
            }
            else if(numberOfJumps > jumpsInARow)
            {
                nextJumpTime = Time.time + jumpCoolDown;
                Debug.Log("Cooldown");
                //what the above line is doing is that it is adding some seconds to the current time so that
                //nextJumpTime becomes greater than Time.time 
                //therefore the player will have to wait for Time.time to catch up with nextJumpTime which will act as a
                //cooldown
            }
        }
        if(Time.time > jumpCompleteReset)
        {
            numberOfJumps = 0;
            Debug.Log("Jumps reset");
        }
        
    }

    private void sprint()
    {
        if(lShiftPressed)
        {
            Vector2 playerMovement = new Vector2();
            playerMovement.x = Input.GetAxis("Horizontal");
            playerMovement.y = Input.GetAxis("Vertical");
            playerMovement.Normalize();

            rb.AddForce(playerMovement.x * sprintForce, rb.velocity.y, playerMovement.y * sprintForce, ForceMode.Impulse);
            Debug.Log("Working");
        }
    }


}

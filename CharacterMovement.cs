using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementForce = 10F;
    public float jumpForce = 3F;
    public float jumpCoolDown;
    public float jumpCounterResetTime;
    public float sprintForce = 20F;
    public bool isGrounded = false;


    [SerializeField]
    private float groundCheckRayLength = 0.02F;
    private int jumpsInARow = 4;
    private float jumpCompleteReset;
    private int numberOfJumps = 0;
    private float nextJumpTime;
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
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded && numberOfJumps <= jumpsInARow)
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                numberOfJumps++;
                jumpCompleteReset = Time.time + jumpCounterResetTime;
            }
            else if(numberOfJumps > jumpsInARow)
            {
                nextJumpTime = Time.time + jumpCoolDown;
                //what the above line is doing is that it is adding some seconds to the current time so that
                //nextJumpTime becomes greater than Time.time 
                //therefore the player will have to wait for Time.time to catch up with nextJumpTime which will act as a
                //cooldown
            }
        }
        if(Time.time > jumpCompleteReset)
        {
            numberOfJumps = 0;
        }
        
    }

    private void sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Vector2 playerMovement = new Vector2();
            playerMovement.x = Input.GetAxis("Horizontal");
            playerMovement.y = Input.GetAxis("Vertical");
            playerMovement.Normalize();

            rb.AddForce(playerMovement.x * sprintForce, rb.velocity.y, playerMovement.y * sprintForce);
            Debug.Log("Working");
        }
    }

    private void userKeyInput()
    {
        Vector2 playerMovement = new Vector2();
        playerMovement.x = Input.GetAxis("Horizontal");
        playerMovement.y = Input.GetAxis("Vertical");
        playerMovement.Normalize();
    }
}

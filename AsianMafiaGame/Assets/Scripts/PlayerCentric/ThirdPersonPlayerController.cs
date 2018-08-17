using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class ThirdPersonPlayerController : NetworkBehaviour
{

    enum MoveState
    {
        Idle,
        Moving,
        Airborne,
        Landing
    }

    public float walkSpeed = 3;
    public float runSpeed = 8;

    [Range(0, 1)]
    private float airControlPercent = 0.1f;

    private MoveState state;

    private readonly float GRAVITY = -15;
    private float jumpHeight = 1;

    private float baseAnimationSpeed;

    private Vector3 velocity = Vector3.zero;
    //float movementSpeed = 0;
    private float currentSpeed = 0;
    private float targetSpeed = 0;
    private float movementSmoothVelocity = 0;
    private float movementSmoothTime = 0.1f;

    //float turnSpeed = 0;
    private float turnSmoothVelocity = 0;
    private float turnSmoothTime = .025f;

    private bool movementEnabled = true;
    private bool running;

    public Transform cameraTransform;
    public CharacterController characterController;

    private Vector3 inputDirection;

    void Start()
    {
        //cameraTransform = Camera.main.transform;
        //controller = GetComponent<CharacterController>();

        state = MoveState.Idle;
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        // Only local player proccesses input
        if (!isLocalPlayer)
        {
            return;
        }

        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
        float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        //
        running = Input.GetKey(KeyCode.LeftShift);

        //
        targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;

        // Set the smoothing time: this value is reduced if the player is in air
        float smoothing = movementSmoothTime / (state == MoveState.Airborne ? airControlPercent : 1);

        // Perform state transitions and state-specific calculations
        if (state == MoveState.Landing)
        {

            targetSpeed = 0; // Player can't move while landing

            state = MoveState.Idle;

        }
        else if (state == MoveState.Idle)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                // Jump
                //velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
                
            }
            else if (currentSpeed > 0)
            {
                // Player started moving
                state = MoveState.Moving;
            }
        }
        else if (state == MoveState.Moving)
        {

            if (!characterController.isGrounded)
            {
                // Player is falling
                state = MoveState.Airborne;
                
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                velocity.y = Mathf.Sqrt(-2 * GRAVITY * jumpHeight);
                state = MoveState.Airborne;

            }
            else if (currentSpeed < 0)
            {
                // Player stopped moving
                state = MoveState.Idle;
            }
          
        }
        else if (state == MoveState.Airborne)
        {
            
            if (characterController.isGrounded)
            {
                // Player has landed
                state = MoveState.Landing;
               
            }
            else
            {
                // Apply gravity
                velocity.y += GRAVITY * Time.deltaTime;
                
            }
        }

        float targetAngle = cameraTransform.eulerAngles.y;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref movementSmoothVelocity, smoothing);

        inputDirection = transform.TransformDirection(inputDirection);
        inputDirection *= targetSpeed;

        characterController.Move((inputDirection + velocity) * Time.deltaTime);

    }

    public void DisableMove()
    {
        movementEnabled = false;
    }

    public void EnableMove()
    {
        movementEnabled = true;
    }

    
}


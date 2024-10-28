using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Code Inspired by - https://www.youtube.com/watch?v=f473C43s8nE&t=1s
//Code Inspired by - https://www.youtube.com/watch?v=xCxSjgYTw9c&t=423s
public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    PlayerControls pc;


    [Header("Debug")]
    public Text debugGrounded;
    public Text debugSpeed;
    public TMP_Text debugState;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float fallSpeed;

    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float fallIncreaseMultiplier;

    public float speedRateOfIncrease;
    public float speedRateOfDecrease;

    public float groundDrag;
    public float airDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    public float diveForce;
    public float diveCooldown;
    private bool readyToDive;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    //[Header("Bounce Pad Handling")]
    //public BouncePad bp;
    //public LayerMask whatIsBouncePad;

    public Transform orientation;

    Vector3 moveDirection;

    public MovementState state;
    private MovementState lastState;
    public enum MovementState {
        walking,
        sprinting,
        crouching,
        sliding,
        falling,
        air
    
    }

    public bool sliding;
    public bool inWater;
    
    void Start()
    {
        EventManager.OnTimerStart();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        pc = GetComponent<PlayerControls>();

        readyToJump = true;
        readyToDive = true;

        startYScale = transform.localScale.y;
    }


    private void StateHandler()
    {
        //Mode - Sliding
        if (sliding && OnSlope())
        {
            debugState.text = "Sliding";
            state = MovementState.sliding;

            if (rb.velocity.y < 0.1f && grounded)
            {
                desiredMoveSpeed = slideSpeed;
            }
        }
        //Mode - Falling
        else if (!grounded && rb.velocity.y < 0.1f && !inWater)
        {
            debugState.text = "Falling";
            state = MovementState.falling;

            desiredMoveSpeed = fallSpeed;

            if (Physics.Raycast(rb.position, Vector3.down, playerHeight * 0.5f + 2f, whatIsGround))
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        //Mode - Crouching
        else if (pc.crouch.IsPressed() && !inWater) 
        {
            debugState.text = "Crouching";
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        //Mode - Sprinting
        else if (grounded && pc.sprint.IsPressed())
        {
            debugState.text = "Sprinting";
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (grounded && readyToJump)
        {
            debugState.text = "Walking";
            state = MovementState.walking;
          
        }
        //Mode - Air
        else
        {
            debugState.text = "Air";
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }

        // Check if desiredMoveSpeed has changed drastically
        // We are checking if we are only transitioning from walking -> sprinting or did we build up a large momentum
        // walking -> sprinting has a difference of 3 hence the greater than 4 check
        //IF we built up momentum then we want to slowly decrease the speed rather than instantly
        if ((Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > (sprintSpeed-walkSpeed+1) && moveSpeed != 0) || (state == MovementState.sliding && lastState == MovementState.air))
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastState = state;
        lastDesiredMoveSpeed = desiredMoveSpeed;

    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly change the move speed to the desired speed
        float time = 0;
        float startValue = moveSpeed;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);

        //This is to adjust how fast you lose movespeed.
        if (desiredMoveSpeed < moveSpeed)
        {
            difference = difference / speedRateOfDecrease;
        }
        //This is to adjust how fast you gain speed
        else
        {
            difference = difference / speedRateOfIncrease;
        }

        //Lerp the speed of the player
        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope() && !inWater)
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else if (state == MovementState.falling && !inWater) {
                float fallSpeed = rb.velocity.y;
                float fallSpeedIncrease = 1+Mathf.Abs(fallSpeed/90f);
                time += Time.deltaTime * speedIncreaseMultiplier * fallSpeedIncrease * fallIncreaseMultiplier;
            }
            else
            {
                time += Time.deltaTime;
            }
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }

 

    private void MyInput()
    {

        //When the user can jump
        if(pc.jump.WasPressedThisFrame() && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        //Start crouch
        if (pc.crouch.WasPressedThisFrame())
        {
            Crouch();
        }
       
        //Stop Crouching
        if (pc.crouch.WasReleasedThisFrame())
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        speedControl();
        StateHandler();
        if (inWater)
        {
            StopAllCoroutines();
        }

        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else{
            rb.drag = airDrag;
        }

    }

    private void MovePlayer()
    {
        Vector2 moveInput = pc.move.ReadValue<Vector2>(); 
        //Calculate the movement direction
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        float speed = walkSpeed;
        if(pc.sprint.IsPressed() && !OnSlope())
        {
            speed = sprintSpeed;
        }

        //On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //On Ground
        else if (grounded)
        {
            //Reduce the speed if the player is moving in the opposite direction
            if (IsOppositeDirectionInput())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * 0.5f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            resetCrouch();
        }

        //in Air
        else if (!grounded)
        {
            //Reduce the speed if the player is moving in the opposite direction
         
                //rb.AddForce(moveDirection.normalized * speed * 10f * 0.5f * airMultiplier, ForceMode.Force);
         
                rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();

    }

    private void FixedUpdate()
    {
        debugSpeed.text = "Speed : " + rb.velocity.magnitude;
        //sprintSpeed = 10;

        //Ground Check
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround))
        {
            debugGrounded.text = "Grounded";
            grounded = true;
        }
        else
        {
            debugGrounded.text = "Not Grounded";
            grounded = false;
        }
        MovePlayer();

    }


    void speedControl() {

        //Reset the speed if the player stops moving
        if (rb.velocity.magnitude <= 3.6 && grounded)
        {
            resetSpeed();
        }

        // Limiting speed on a slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit the velocity when needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;//Calculate what the max velocity is
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);//Apply that velocity
            }
        }
    }

    //Allows the player to jump
    private void Jump()
    {
        exitingSlope = true;
        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    //Allows user to crouch
    private void Crouch() {

        //If grounded we crouch
        if (grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
        //If we are in the air we dive
        else if (readyToDive && !inWater)
        {
            readyToDive = false;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
            
        }
    }
    private void resetCrouch() {
        readyToDive = true;
    }

    public void resetSpeed()
    {
        //If the player starts walking or stops moving then we reset their speed
        desiredMoveSpeed = walkSpeed;
        moveSpeed = walkSpeed;
        StopAllCoroutines();
    }

    private bool IsOppositeDirectionInput()
    {
        //Get the players input
        Vector2 moveInput = pc.move.ReadValue<Vector2>();
        Vector3 inputDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        //Which direction the player wants to move
        inputDirection.Normalize();

        // Which direction the player is currently moving
        Vector3 currentDirection = rb.velocity.normalized;

        // How do these directions compare
        float dotProduct = Vector3.Dot(currentDirection, inputDirection);

        // If the dot product is negative, the input direction is opposite to the current movement direction
        return dotProduct < 0;
    }


    //We check if the player is on a slope
    public bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }   
        return false;
    }

    //We get the direction of the slope
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}


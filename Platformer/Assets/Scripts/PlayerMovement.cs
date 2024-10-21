using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


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

        startYScale = transform.localScale.y;
    }


    private void StateHandler()
    {

        //Mode - Sliding
        if (sliding && grounded)
        {
            debugState.text = "Sliding";
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f && grounded)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
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
        else if (pc.crouch.IsPressed()) 
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
        else if (grounded)
        {
            debugState.text = "Walking";
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
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
        if (pc.crouch.IsPressed())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);


            //Can look to include this code, it will basically stop it from forcing you downwards when you are midair
            //This effect can be good or bad, depends on the scenario
            //if (grounded) {
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            //}
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
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        //in Air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();

    }

    private void FixedUpdate()
    {
        debugSpeed.text = "Speed : " + rb.velocity.magnitude;


        //Ground Check
        //if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsBouncePad)) {
        //    bp.bouncePlayer(rb);
        //}
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

    public bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }   
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
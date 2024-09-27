using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;


    [Header("Debug")]
    public Text debugGrounded;
    public Text debugSpeed;
    public TMP_Text debugState;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float speedRateOfIncrease;
    public float speedRateOfDecrease;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Bounce Pad Handling")]
    public BouncePad bp;
    public LayerMask whatIsBouncePad;




    public Transform orientation;

    float horizontaltInput;
    float verticalInput;

    Vector3 moveDirection;

    public MovementState state;
    public enum MovementState {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    
    }

    public bool sliding;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


        readyToJump = true;

        startYScale = transform.localScale.y;
    }


    private void StateHandler()
    {

        //Mode - Sliding
        if (sliding && grounded)
        {
            debugState.text = "Sliding";
            Debug.Log("WE are sliding");

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
        //Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            debugState.text = "Crouching";
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        //Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
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

            if(rb.velocity.y < 0.1f)
            {
                //Need to change this part to make the slide epic
                desiredMoveSpeed = 50f;
            }
        }

        // Check if desiredMoveSpeed has changed drastically
        // We are checking if we are only transitioning from walking -> sprinting or did we build up a large momentum
        // walking -> sprinting has a difference of 3 hence the greater than 4 check
        //IF we built up momentum then we want to slowly decrease the speed rather than instantly
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4 && moveSpeed != 0)
        {
            Debug.Log("We are lerping");
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

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

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
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
        horizontaltInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When the user can jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        //Start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);


            //Can look to include this code, it will basically stop it from forcing you downwards when you are midair
            //This effect can be good or bad, depends on the scenario
            //if (grounded) {
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            //}
        }

        //Stop Crouching
        if (Input.GetKeyUp(crouchKey))
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

        //DEBUG START
        if (sliding)
        {
            EventManager.OnTimerStart();
        }
        else
        {
            EventManager.OnTimerReset();
            EventManager.OnTimerStop();
        }
        //DEBUG END

        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }

    }

    private void MovePlayer()
    {
        //Calculate the movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontaltInput;

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
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsBouncePad)) {
            bp.bouncePlayer(rb);
        }
        else if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround))
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

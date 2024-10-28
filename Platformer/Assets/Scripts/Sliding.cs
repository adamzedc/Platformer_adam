using UnityEngine;
using UnityEngine.UI;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;
    private PlayerControls pc;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private Vector2 moveInput;
    private bool slideBuffer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        pc = GetComponent<PlayerControls>();

        //slideMultiplier = 1f;
       // highestMultiplier = 1f;
        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        moveInput = pc.move.ReadValue<Vector2>();

        //Buffer the slide so that the player can slide the moment they touch the ground
        if (pc.sprint.WasPressedThisFrame()){
            slideBuffer = true;
        }
        //Checking if the user can slide
        if (slideBuffer && (moveInput.x != 0 || moveInput.y != 0) && pm.grounded) {
            slideBuffer = false;
            Debug.Log("Sliding");
            StartSlide();
            
        }

        //Player will stop sliding when they press a new input (This allows the user to swap to sprinting or jumping whilst still keeping the speed from sliding)
        if (pc.jump.WasPressedThisFrame() && pm.sliding) {
            StopSlide();
        }


    }

    private void FixedUpdate()
    {   
        //if (!pm.OnSlope() && rb.velocity.y < -6f) updateSlideMultiplier();

        if (pm.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        pm.sliding = true;
        
        //Changing the Y scale so that it looks like we are crouched down sliding
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        
        rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        //Allows us to slide in any direction
        Vector3 inputDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        ////Sliding Normally
        //if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        //{
        //    rb.AddForce(inputDirection * (slideForce), ForceMode.Force);

        //    //Count down the timer, when time runs out, stop sliding
        //    //Sliding at high speeds (Should slide until speed goes below 12)
        //    if (rb.velocity.magnitude < 12)
        //    {
        //        slideTimer -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        //Debug.Log("We are sliding at high speeds : " + rb.velocity.magnitude);
        //    }
        //}

        //Sliding down slope
        //else {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        //}

        if (!pm.OnSlope())
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;
        Debug.Log("Stopped Sliding");
        //Resetting scale as we are no longer sliding
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}

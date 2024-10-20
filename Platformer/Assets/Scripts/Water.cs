using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("References")]
    public LayerMask whatIsWater;
    private Rigidbody rb;
    private PlayerMovement pm;
    private Sliding sl;

    [Header("Movement")]
    public float waterGroundDrag;
    public float waterXAirDrag;
    public float waterYAirDrag;
    public float waterGravity;
    public float waterDownwardForce;
    private float defaultGravity;
    //public float waterWalkSpeed;
    //public float waterSprintSpeed;
    

    private float airDrag;
    private float groundDrag;
    //private float walkSpeed;
    //private float sprintSpeed;




    // Start is called before the first frame update

    // Update is called once per frame

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        sl = GetComponent<Sliding>();


        defaultGravity = -9.81f;
        airDrag = pm.airDrag;
        groundDrag = pm.groundDrag;
        //walkSpeed = pm.walkSpeed;
        //sprintSpeed = pm.sprintSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water")
        {
            Vector3 dragVel = transform.InverseTransformDirection(rb.velocity);
            //Applying drag to y axis, only downward drag
            //Applying downwards drag the moment the player enters the water
            if (rb.velocity.y < 0f)
            {
                dragVel.y = dragVel.y * (1 - Time.deltaTime * waterYAirDrag);
                rb.velocity = transform.TransformDirection(dragVel);
            }


            pm.inWater = true;
            Debug.Log("We are in water");
            //pm.walkSpeed = waterWalkSpeed;
            //pm.sprintSpeed = waterSprintSpeed;
            
            pm.groundDrag = waterGroundDrag;
            Physics.gravity = new Vector3(0,-waterGravity,0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "water")
        {
            pm.inWater = false;
            Debug.Log("We have left the water");
            //pm.walkSpeed = walkSpeed;
            //pm.sprintSpeed = sprintSpeed;

            pm.airDrag = airDrag;
            pm.groundDrag = groundDrag;
            Physics.gravity = new Vector3(0, defaultGravity, 0);
        }

        
    }

    private void FixedUpdate()
    {
        if (pm.inWater && !pm.grounded) {
            rb.AddForce(Vector3.down * waterDownwardForce, ForceMode.Force);

            //rb.velocity = rb.velocity * (1 - Time.deltaTime * waterAirDrag);

            //Applying drag in all directions other than upwards
            //This will allow the player to feel like they are in water
            // whilst still being able to jump as high
            Vector3 dragVel =  transform.InverseTransformDirection(rb.velocity);

            //Applying a little drag to y axis
            if (rb.velocity.y < 0f)
            {
                dragVel.y = dragVel.y * (1 - Time.deltaTime * 1);
            }

            //Applying drag to x axis
            dragVel.x = dragVel.x * (1 - Time.deltaTime * waterXAirDrag);


            //Applying drag to z axis
            dragVel.z = dragVel.z * (1 - Time.deltaTime * waterXAirDrag);

            rb.velocity = transform.TransformDirection(dragVel);
            rb.angularDrag = waterXAirDrag;
        }
    }
}

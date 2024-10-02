using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("References")]
    public LayerMask whatIsWater;
    public Rigidbody rb;
    public PlayerMovement pm;
    public Sliding sl;

    [Header("Movement")]
    public float waterGroundDrag;
    public float waterAirDrag;
    public float waterGravity;
    public float waterWalkSpeed;
    public float waterSprintSpeed;
    

    private float airDrag;
    private float groundDrag;
    private float walkSpeed;
    private float sprintSpeed;



    // Start is called before the first frame update

    // Update is called once per frame

    private void Start()
    {
        airDrag = pm.airDrag;
        groundDrag = pm.groundDrag;
        walkSpeed = pm.walkSpeed;
        sprintSpeed = pm.sprintSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water")
        {
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
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

        
    }

    void FixedUpdate()
    {
        if (pm.inWater) { 
            //rb.velocity = rb.velocity * (1 - Time.deltaTime * waterAirDrag);
            
            Vector3 dragVel =  transform.InverseTransformDirection(rb.velocity);
            dragVel.x = dragVel.x * (1 - Time.deltaTime * waterAirDrag);
            if (rb.velocity.y < 0f)
            {
                dragVel.y = dragVel.y * (1 - Time.deltaTime * waterAirDrag);
            }
            dragVel.z = dragVel.z * (1 - Time.deltaTime * waterAirDrag);

            rb.velocity = transform.TransformDirection(dragVel);
        }
    }
}

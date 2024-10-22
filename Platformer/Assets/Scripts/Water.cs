using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("References")]
    public LayerMask whatIsWater;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Movement")]
    public float waterGroundDrag;
    public float waterXAirDrag;
    public float waterYAirDrag;
    public float waterGravity;
    public float waterDownwardForce;
    private float defaultGravity;

    private float airDrag;
    private float groundDrag;

    private int waterCounter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();


        defaultGravity = -9.81f;
        airDrag = pm.airDrag;
        groundDrag = pm.groundDrag;
        waterCounter = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water")
        {
            waterCounter++;

            //We should only apply effects to the player once they have entered the water
            //It should not be applied multiple times with different water triggers
            if (waterCounter == 1)
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

                pm.groundDrag = waterGroundDrag;
                Physics.gravity = new Vector3(0, -waterGravity, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //We should only leave the water if we have left all water triggers
        if (other.tag == "water")
        {
            waterCounter--;
            if (waterCounter == 0)
            {
                pm.inWater = false;
            }
            Debug.Log("We have left the water");
        }


    }

    private void FixedUpdate()
    {
        float sphereRadius = pm.playerHeight * 0.5f + 0.1f;
        Vector3 origin = transform.position;

        // Check for water
        Collider[] hitColliders = Physics.OverlapSphere(origin, sphereRadius, whatIsWater);
        bool inWater = false;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("water"))
            {
                inWater = true;
                break;
            }
        }

        // If we are not in water but the player is in water, we should leave the water
        //This will allow us to react to the water suddenly disappearing, due to our secret mechanic
        //The water suddenly disappearing does not trigger OnTriggerExit
        if (!inWater && pm.inWater)
        {
            LeaveWater();
        }

        if (pm.inWater && !pm.grounded)
        {
            rb.AddForce(Vector3.down * waterDownwardForce, ForceMode.Force);

            //Applying drag in all directions other than upwards
            //This will allow the player to feel like they are in water
            // whilst still being able to jump as high
            Vector3 dragVel = transform.InverseTransformDirection(rb.velocity);

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


    private void LeaveWater()
    {
        //Resetting the player's values to their default values
        pm.inWater = false;
        pm.airDrag = airDrag;
        pm.groundDrag = groundDrag;
        Physics.gravity = new Vector3(0, defaultGravity, 0);
    }
}

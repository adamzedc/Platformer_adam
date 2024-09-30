using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("References")]
    public LayerMask whatIsWater;
    public Rigidbody rb;
    public PlayerMovement pm;
    
    
    
    // Start is called before the first frame update

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water")
        {
            Debug.Log("We are in water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "water")
        {
            Debug.Log("We have left the water");
        }

        
    }

    void FixedUpdate()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectsOnTrain : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    private bool objectOnTrain;
    private PlayerMovement pm;
    private Vector3 lastTrainPosition;
    private GameObject obj;
    private float playerSpeed;
    private Rigidbody objRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        objectOnTrain = false;
    }

    private void Update()
    {
        if (objectOnTrain)
        {
            ApplyTrainMovement();
        }
    }

    // TRAIN MOVEMENT TRACKING
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            obj = collision.gameObject;
            pm = obj.GetComponent<PlayerMovement>();
            Debug.Log("HELLO SIR");
            objectOnTrain = true;
            lastTrainPosition = rb.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            obj = collision.gameObject;
            Debug.Log("GOODBYE SIR");
            //objRb = obj.GetComponent<Rigidbody>();
            playerSpeed = pm.desiredMoveSpeed;
            //objRb.AddForce(Vector3.forward * agent.speed, ForceMode.Impulse);
            pm.desiredMoveSpeed = playerSpeed + agent.speed;
            
            objectOnTrain = false;
        }
    }

    private void ApplyTrainMovement()
    {
        // Calculate the train's movement since the last frame
        Vector3 trainMovement = rb.position - lastTrainPosition;

        // Move the player along with the train's movement
        objRb = obj.GetComponent<Rigidbody>();
        objRb.MovePosition(objRb.position + trainMovement);

        // Update the last position for the next frame
        lastTrainPosition = rb.position;
    }

    private void AdjustPlayerMoveSpeedOnExit()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float playerJumpForce;
    private float trainSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        objectOnTrain = false;
        lastTrainPosition = rb.position;
    }

    private void Update()
    {
        if (objectOnTrain)
        {
            //This will cause the player to move with the train
            ApplyTrainMovement();
            // Update the last position for the next frame
        }
        trainSpeed = ((rb.position - lastTrainPosition) / Time.deltaTime).magnitude;
        lastTrainPosition = rb.position;

    }

    // TRAIN MOVEMENT TRACKING
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            //Initialize key variables to calculate interaction between player and train
            obj = other.transform.parent.gameObject;
            pm = obj.GetComponent<PlayerMovement>();
            playerSpeed = pm.sprintSpeed;
            Debug.Log("HELLO SIR");
            objectOnTrain = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            //Get the direction of both objects
            Vector3 trainDirection = ((rb.position - lastTrainPosition) / 2).normalized;
            Vector3 playerDirection = (other.transform.position - lastTrainPosition).normalized;

            //Calculate the dot product of the two directions
            float dotProduct = Vector3.Dot(trainDirection, playerDirection);

            //Set the similarity threshold
            float similarityThreshold = 0.2f;

            //If the player is moving in the same direction as the train
            //Then we will give the player a speed boost
            if (dotProduct > similarityThreshold) {
                Debug.Log("Player is moving in the same direction as the train");
                AdjustPlayerMoveSpeed();
                //Reset the speed after a short delay
                Invoke("ResetPlayerMovement", 0.1f);
            }
            objectOnTrain = false;
        }
    }

    private void ApplyTrainMovement()
    {
        // Calculate the train's movement since the last frame
        Vector3 trainMovement = rb.position - lastTrainPosition;
        //Debug.Log(trainMovement/Time.deltaTime);

        // Move the player along with the train's movement
        objRb = obj.GetComponent<Rigidbody>();
        objRb.MovePosition(objRb.position + trainMovement);
    }

    private void AdjustPlayerMoveSpeed()
    {
        //Gives the player a speed boost based on the train's speed
        Debug.Log("Train Vector Speed: " + (rb.position - lastTrainPosition)/Time.deltaTime);
        Debug.Log("Train Speed: " + trainSpeed);

        pm.sprintSpeed = playerSpeed + trainSpeed;
        pm.desiredMoveSpeed = playerSpeed + trainSpeed;
    }

    private void ResetPlayerMovement()
    {
        //Resets the player's speed back to normal
        pm.sprintSpeed = playerSpeed;
    }
}

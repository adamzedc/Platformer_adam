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
            Debug.Log("Player has entered the train");
            obj = other.transform.parent.gameObject;
            pm = obj.GetComponent<PlayerMovement>();
            playerSpeed = pm.sprintSpeed;
            objectOnTrain = true;
            pm.resetSpeed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            Debug.Log("Player has left the train");
            //Get the direction of both objects
            Vector3 trainDirection = (rb.position - lastTrainPosition).normalized;
            Vector3 playerDirection = (other.transform.position - lastTrainPosition).normalized;

            //Calculate the dot product of the two directions
            float dotProduct = Vector3.Dot(trainDirection, playerDirection);

            //Set the similarity threshold
            float similarityThreshold = -0.25f;

            //If the player is moving in the same direction as the train
            //Then we will give the player a speed boost
            if (dotProduct > similarityThreshold) {
                AdjustPlayerMoveSpeed();
                //Reset the speed after a short delay
                Invoke(nameof(ResetPlayerMovement), 0.1f);
            }
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
    }

    private void AdjustPlayerMoveSpeed()
    {
        //Gives the player a speed boost based on the train's speed
        // Calculate the train's movement vector
        Vector3 trainMovement = (rb.position - lastTrainPosition) / Time.deltaTime;

        // Get the player's Rigidbody component
        objRb = obj.GetComponent<Rigidbody>();

        // Calculate the boost force by adding the train's movement vector to the player's current velocity
        Vector3 boostForce = objRb.velocity + trainMovement;
        boostForce.y = 0;

        //Add a force to the player equal to the train's speed
        objRb.AddForce(boostForce, ForceMode.VelocityChange);

        //Adjust the player's speed to match the train's speed
        pm.sprintSpeed = Mathf.Clamp(playerSpeed + trainSpeed,10,20);
        pm.desiredMoveSpeed = Mathf.Clamp(playerSpeed + trainSpeed,10,20);
    }

    private void ResetPlayerMovement()
    {
        //Resets the player's speed back to normal
        pm.sprintSpeed = playerSpeed;
    }
}

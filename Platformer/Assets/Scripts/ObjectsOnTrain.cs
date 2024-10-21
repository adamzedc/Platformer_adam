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
            //AdjustPlayerMoveSpeed();
        }
        
    }

    // TRAIN MOVEMENT TRACKING

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("player"))
        // {
        //     AdjustPlayerJumpForce();
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            obj = other.transform.parent.gameObject;
            pm = obj.GetComponent<PlayerMovement>();
            //playerJumpForce = pm.jumpForce;
            playerSpeed = pm.sprintSpeed;
            Debug.Log("HELLO SIR");
            objectOnTrain = true;
            lastTrainPosition = rb.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            //obj = collision.gameObject;
            Debug.Log("GOODBYE SIR");
            //objRb = obj.GetComponent<Rigidbody>();
            //playerSpeed = pm.moveSpeed;
            //objRb.AddForce(Vector3.forward * agent.speed*2, ForceMode.Impulse);
            //pm.moveSpeed = playerSpeed + agent.speed * 100;
            AdjustPlayerMoveSpeed();
            objectOnTrain = false;
            Invoke("ResetPlayerMovement",0.1f);
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

    private void AdjustPlayerMoveSpeed()
    {
        //playerSpeed = pm.sprintSpeed;
        pm.sprintSpeed = playerSpeed + agent.speed;
        pm.desiredMoveSpeed = playerSpeed + agent.speed;
    }

    private void AdjustPlayerJumpForce()
    {
        //pm.jumpForce = playerJumpForce + agent.speed;
    }

    private void ResetPlayerMovement()
    {
        pm.sprintSpeed = playerSpeed;
        //pm.jumpForce = playerJumpForce;
    }
}

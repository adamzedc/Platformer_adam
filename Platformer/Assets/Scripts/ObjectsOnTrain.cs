using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsOnTrain : MonoBehaviour
{
    private Rigidbody rb;
    private bool objectOnTrain;
    private Vector3 lastTrainPosition;
    private GameObject obj;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectOnTrain = false;
    }

    private void Update()
    {
        if (objectOnTrain) {
            ApplyTrainMovement();
        }
    }

    // TRAIN MOVEMENT TRACKING
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            obj = collision.gameObject;
            Debug.Log("HELLO SIR");
            objectOnTrain = true;
            lastTrainPosition = rb.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            Debug.Log("GOODBYE SIR");
            objectOnTrain = false;
        }
    }

    private void ApplyTrainMovement()
    {
        // Calculate the train's movement since the last frame
        Vector3 trainMovement = rb.position - lastTrainPosition;

        // Move the player along with the train's movement
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        objRb.MovePosition(objRb.position + trainMovement);

        // Update the last position for the next frame
        lastTrainPosition = rb.position;
    }
}

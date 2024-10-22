using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularEnemyMovement : MonoBehaviour
{
    public float speed = 5f;         // Speed of movement
    public float radius = 3f;        // Radius of the circular path

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the x and y positions for circular movement
        float offsetX = Mathf.Cos(Time.time * speed) * radius;
        float offsetY = Mathf.Sin(Time.time * speed) * radius;

        // Apply the circular movement in x and z axes
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y  + offsetY, startPosition.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularEnemyMovement : MonoBehaviour
{
    public float speed = 5f;        
    public float radius = 3f;  // this is the radius of the circle in which the enemy will move.

    private Vector3 startPosition;

    void Start()
    {
        // initial position.
        startPosition = transform.position;
    }

    void Update()
    {
        // uses sin and cos to calculate the x and y positions in a circular manner
        float offsetX = Mathf.Cos(Time.time * speed) * radius;
        float offsetY = Mathf.Sin(Time.time * speed) * radius;

        // circular movement applied in the x and y axes.
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y  + offsetY, startPosition.z);
    }
}

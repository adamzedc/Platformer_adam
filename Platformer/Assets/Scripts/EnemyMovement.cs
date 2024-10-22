using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;         // the speed
    public float distance = 3f;      // how far it moves

    private Vector3 startPosition;   

    void Start()
    {
        // initial position that is pingponged around
        startPosition = transform.position;
    }

    void Update()
    {
        // left and right movement using pingpong
        float offsetX = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // apply movement in x axis.
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y, startPosition.z);
    }
}
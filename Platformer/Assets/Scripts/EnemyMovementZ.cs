using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementZ : MonoBehaviour
{
    public float speed = 5f;         // the speed
    public float distance = 3f;     // how far it moves

    private Vector3 startPosition;   

    void Start()
    {
        // start pos
        startPosition = transform.position;
    }

    void Update()
    {
        // left and right movement using pingpong
        float offsetZ = Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // apply movement in z axis.
        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z + offsetZ);
    }
}
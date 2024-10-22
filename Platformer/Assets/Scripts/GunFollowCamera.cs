using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; 
    public Vector3 offset;            

    void FixedUpdate()
    {
        // the gun rotates with the camera.
        transform.rotation = cameraTransform.rotation;

        // gun is also offset with the camera.
        transform.position = cameraTransform.position + cameraTransform.forward * offset.z + cameraTransform.right * offset.x + cameraTransform.up * offset.y;
    }
}

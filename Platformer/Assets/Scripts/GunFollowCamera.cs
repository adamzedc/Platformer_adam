using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the Camera's transform
    public Vector3 offset;            // Offset to position the gun correctly relative to the camera

    void FixedUpdate()
    {
        // Match the gun's rotation to the camera's rotation
        transform.rotation = cameraTransform.rotation;

        // Optionally, position the gun with an offset relative to the camera
        transform.position = cameraTransform.position + cameraTransform.forward * offset.z + cameraTransform.right * offset.x + cameraTransform.up * offset.y;
    }
}

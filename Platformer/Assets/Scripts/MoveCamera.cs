using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform cameraPosition;

    // updates the camera position every frame.
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public Camera cam;

    [Header("Sensitivity")]
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    [Header("fovChange")]
    public float fovAdditiveMultiplier;
    public float minFov;
    public float maxFov;
    private float fovMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxisRaw("MouseX") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("MouseY") * Time.deltaTime * sensY;

        if(rb.velocity.magnitude > 12)
        {
            //fovChange();
        }

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void FixedUpdate()
    {
    }


    //Need to lerp the fov values so that they dont suddenly drastically change
    private void fovChange() {
        fovMultiplier = 1 + ((rb.velocity.magnitude - 10f) * fovAdditiveMultiplier);
        float fovValue = 50 * fovMultiplier;
        cam.fieldOfView = Mathf.Clamp(fovValue, minFov, maxFov);
    }

}

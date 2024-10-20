using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Death : MonoBehaviour
{
    [Header("Mandatory Values")]
    private Rigidbody rb;
    public float playerHeight;
    public LayerMask whatIsDanger;

    [Header("DEBUG")]
    public TMP_Text debugDeadState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //When you touch the danger ground, you die
        if(Physics.Raycast(rb.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsDanger))
        {
            debugDeadState.text = "Dead";
            SceneManager.LoadScene("Menu");
        }
    }
}

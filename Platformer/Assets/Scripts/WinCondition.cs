using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winUI;

    //Maybe try to reference UI without having it to be a public object
    //Think bout it
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "win")
        {
            winUI.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene("Menu"); // loads the menu when you win for now. will be a win screen in the full game.

        }
    }
}

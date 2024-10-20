using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        // Make the cursor visible
        Cursor.visible = true;
    }
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame(){
        Debug.Log("Quit!");
        Application.Quit();
    }
}

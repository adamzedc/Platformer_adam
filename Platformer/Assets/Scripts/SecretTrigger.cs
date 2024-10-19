using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretTrigger : MonoBehaviour
{
    //Explanation on how to use!
    //Create a secret part of the level, store it all under 1 game object
    //Store the game object under the secretLevel gameObject variable
    //Make the game object invisible
    //It should work now!


    public GameObject secretLevel;

    //Make the level visible when the target has been hit!
    public void OnTargetHit()
    {
        secretLevel.SetActive(true);
    }
}

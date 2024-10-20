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
    private enum secretTypes
    {
        ObjectGoesFromInvisibleToVisible,
        ObjectGoesFromVisibleToInvisible
    }
    [SerializeField]
    private secretTypes secretType;


    //Make the level visible when the target has been hit!
    public void OnTargetHit()
    {
        //With the use of the enum, we can choose whether we want an object to disappear when we hit the target
        //OR
        //We can choose to make it appear when we hit the target

        if (secretTypes.ObjectGoesFromInvisibleToVisible == secretType)
        {
            secretLevel.SetActive(true);
        }
        else if (secretTypes.ObjectGoesFromVisibleToInvisible == secretType) { 
            secretLevel.SetActive(false);
        }
    }
}

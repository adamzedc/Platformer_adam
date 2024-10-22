using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private SecretTrigger secretTrigger;

    //Check if the bullet has collided with something
    private void OnCollisionEnter(Collision collision){

        //If we hit an enemy
        if (collision.gameObject.CompareTag("enemy"))
        {
            print("hit" + collision.gameObject.name + "!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
            BarEventManager.OnSliderReset();
            ScoreEventManager.OnScoreIncrement();
        }
        //If we hit a secret
        else if (collision.gameObject.CompareTag("secret"))
        {
            secretTrigger = collision.gameObject.GetComponent<SecretTrigger>();
            Destroy(collision.gameObject);
            secretTrigger.OnTargetHit();
        }
        //If we hit a hitbox
        else if (collision.gameObject.CompareTag("hitbox"))
        {
            print("hit" + collision.gameObject.transform.parent.name + "'s hitbox!");
            if (collision.gameObject.transform.parent != null)
            {
                // this will destroy the hitbox's parent
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
            Destroy(gameObject);
            BarEventManager.OnSliderReset();
            ScoreEventManager.OnScoreIncrement();
        }
       
    }
}

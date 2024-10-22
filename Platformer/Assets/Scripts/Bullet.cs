using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private SecretTrigger secretTrigger;

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("enemy"))
        {
            print("hit" + collision.gameObject.name + "!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
            BarEventManager.OnSliderReset();
        }
        else if (collision.gameObject.CompareTag("secret"))
        {
            secretTrigger = collision.gameObject.GetComponent<SecretTrigger>();
            Destroy(collision.gameObject);
            secretTrigger.OnTargetHit();
        }
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
        }
        else {
            Destroy(gameObject);
        }
    }
}

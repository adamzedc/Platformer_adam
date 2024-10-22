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
            Debug.Log("Bullet Collision");
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
        else {
            Destroy(gameObject);
        }
    }
}

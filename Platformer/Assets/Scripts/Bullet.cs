using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject gun;
    private Weapon weapon;
    private SecretTrigger secretTrigger;

    private void Start() {
        gun = transform.parent.gameObject;
        weapon = gun.GetComponent<Weapon>();
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("Bullet Collision");
            print("hit" + collision.gameObject.name + "!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
            weapon.hit = true;
        }
        else if (collision.gameObject.CompareTag("secret")) {
            secretTrigger = collision.gameObject.GetComponent<SecretTrigger>();
            Destroy(collision.gameObject);
            secretTrigger.OnTargetHit();
        }
    }
}

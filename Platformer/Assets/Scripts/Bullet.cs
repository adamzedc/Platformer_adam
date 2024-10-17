using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject gun;
    private Weapon weapon;

    private void Start() {
        gun = transform.parent.gameObject;
        weapon = gun.GetComponent<Weapon>();
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("enemy")){ 
            Debug.Log("Bullet Collision");
            print("hit"+ collision.gameObject.name + "!"); 
            Destroy(collision.gameObject);
            Destroy(gameObject);
            weapon.hit = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("enemy")){ 
            print("hit"+ collision.gameObject.name + "!"); 
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

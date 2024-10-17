using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;
    public PlayerInputActions playerControls;
    public InputAction fire;
    public bool hit;

    void Awake()
    {
        playerControls = new PlayerInputActions();
        hit = false;
    }

    private void OnEnable(){
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable(){
        fire.Disable();
    }
    private void Fire(InputAction.CallbackContext context){
        //Create the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.SetParent(transform);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay){
        //Destroy the bullet after some time
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

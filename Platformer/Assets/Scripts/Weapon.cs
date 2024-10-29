using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public int bulletCount = 5;               // this is the number of bullets per shot. this is default 5 but can be set to whatever in unity.
    public float spreadAngle = 15f;           // spread angle. can also be adjusted in unity.

    private PlayerInputActions playerControls;
    private InputAction fire;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    private void Fire(InputAction.CallbackContext context)
    {
        for (int i = 0; i < bulletCount; i++)
        {

            Quaternion spreadRotation = Quaternion.Euler( // creates spread (if there is any.)
                Random.Range(-spreadAngle, spreadAngle),  
                Random.Range(-spreadAngle, spreadAngle),  
                0);                                       

            // create the bullet
            Quaternion bulletRotation = bulletSpawn.rotation * spreadRotation;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletRotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletVelocity, ForceMode.Impulse);

            // destroy the bullet after some time.
            StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        }
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

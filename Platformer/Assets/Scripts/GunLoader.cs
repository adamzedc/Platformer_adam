using UnityEngine;

public class GunLoader : MonoBehaviour
{
    // the gun prefabs.
    public GameObject PistolPrefab;
    public GameObject ShotgunPrefab;

    private GameObject currentGun;  // currently equipped gun.
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;  // the player camera.
        LoadGun();
    }

    // this will load the gun from settings and instantiate it.
    public void LoadGun()
    {
        if (currentGun != null)
        {
            Destroy(currentGun);
        }

        // gets the gun id thats saved in settings.
        int selectedGunID = SettingsManager.LoadSelectedGun();

        switch (selectedGunID)
        {
            case 0:  // Pistol 
                currentGun = Instantiate(PistolPrefab, transform);
                break;
            case 1:  // Shotgun
                currentGun = Instantiate(ShotgunPrefab, transform);
                break;
            default:
                Debug.LogWarning("Unknown gun ID: " + selectedGunID);
                currentGun = Instantiate(PistolPrefab, transform);  // defaults to pistol if something is wrong in the settings.
                break;
        }

        // this will attach the player camera to the script on the gun that makes it follow the camera.
        var gunFollowCamera = currentGun.GetComponent<GunFollowCamera>();
        if (gunFollowCamera != null)
        {
            gunFollowCamera.cameraTransform = playerCamera;
        }
    }
}

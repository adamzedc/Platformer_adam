using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    
    private const string SelectedGunKey = "SelectedGun";

    // SETTERS
    public static void SaveSelectedGun(int gunID) // this saves the selected gun. currently not used.
    {
        PlayerPrefs.SetInt(SelectedGunKey, gunID);
        PlayerPrefs.Save();  
    }

    // GETTERS
    public static int LoadSelectedGun() // this loads the selected gun. 0 is pistol and 1 is shotgun. just change the number for now and it will load 
    {
        
        return PlayerPrefs.GetInt(SelectedGunKey, 1);
    }
}

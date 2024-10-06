using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    [Header("Train Settings")]
    public GameObject trainPrefab;          // The train prefab to spawn
    public Cinemachine.CinemachinePathBase dollyTrack; // Reference to the Cinemachine Dolly Track
    public float minSpawnInterval = 5f;     // Minimum time between spawns
    public float maxSpawnInterval = 15f;    // Maximum time between spawns
    public float trainSpeed = 5f;           // Speed of the trains

    [Header("Spawner Settings")]
    public int maxTrains = 10;              // Maximum number of trains in the scene
    private int currentTrains = 0;

    void Start()
    {
        StartCoroutine(SpawnTrains());
    }

    IEnumerator SpawnTrains()
    {
        while (currentTrains < maxTrains)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnTrain();
        }
    }

    void SpawnTrain()
    {
        // Instantiate the train at the spawn point's position and rotation
        
        GameObject newTrain = Instantiate(trainPrefab, transform.position, transform.rotation);

        // Get the Cinemachine Dolly Cart component
        Cinemachine.CinemachineDollyCart dollyCart = newTrain.GetComponent<Cinemachine.CinemachineDollyCart>();

        if (dollyCart != null)
        {
            dollyCart.m_Path = dollyTrack;    // Assign the Dolly Track
            dollyCart.m_Speed = trainSpeed;   // Set the speed
            dollyCart.m_Position = 0f;        // Start at the beginning of the track
        }
        else
        {
            Debug.LogError("Train prefab does not have a CinemachineDollyCart component.");
        }

        currentTrains++;
    }

    // Optional: If you want to handle train destruction and decrement the count
    public void TrainDestroyed()
    {
        if (currentTrains > 0)
            currentTrains--;
    }
}
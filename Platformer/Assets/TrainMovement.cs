using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrainMovement : MonoBehaviour
{

    private float trainSpeed;
    private NavMeshAgent agent;
    public float minTrainSpeed;
    public float maxTrainSpeed;

    // Start is called before the first frame update

    //This will randomise the speed of each train
    //Randomising the speed should alter their pathing as some trains will need to go around others
    void Start()
    {
        trainSpeed = Random.Range(minTrainSpeed, maxTrainSpeed);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = trainSpeed;
    }
}

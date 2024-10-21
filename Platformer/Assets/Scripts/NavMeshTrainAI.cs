using UnityEngine;
using UnityEngine.AI;

public class NavMeshTrainAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    private TrainGroupManager groupManager; // the train's group

    private void Start()
    {
        // navmesh disabled initiallty
        if (agent != null)
        {
            agent.enabled = false;
        }

        // find the train's group manager
        groupManager = GetComponentInParent<TrainGroupManager>();
    }

    private void FixedUpdate()
    {
        if (agent != null && agent.enabled && target != null)
        {
            agent.SetDestination(target.position); // navmesh moving towards the target.
        }
    }

    // Detect when the player jumps onto the train
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("player"))
        {
            // tell the group manager to start all the trains
            if (groupManager != null)
            {
                groupManager.ActivateAllTrains(); // move all the trains
            } else{
                Debug.Log("group manager null");
            }
        }
    }
}

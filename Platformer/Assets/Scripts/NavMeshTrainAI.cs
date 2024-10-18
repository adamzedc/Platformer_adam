using UnityEngine;
using UnityEngine.AI;

public class NavMeshTrainAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;

    

    private void FixedUpdate() {
        if (target != null){
            // Ensure that we are setting the destination only when the NavMeshAgent is valid
            if (agent != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}

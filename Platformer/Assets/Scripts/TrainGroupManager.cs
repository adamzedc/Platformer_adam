using UnityEngine;
using UnityEngine.AI;

public class TrainGroupManager : MonoBehaviour
{
    private bool trainsMoving = false; 

    // this will start all the trains in the group.
    public void ActivateAllTrains()
    {
        if (!trainsMoving) 
        {
            foreach (Transform child in transform) // goes through each child train to start it.
            {
                NavMeshAgent childAgent = child.GetComponent<NavMeshAgent>();
                if (childAgent != null)
                {
                    childAgent.enabled = true; // start the train
                }
            }

            trainsMoving = true; 
            
        }
    }
}

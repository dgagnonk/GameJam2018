using UnityEngine;
using UnityEngine.AI;

public class NPCNavmeshAgent : MonoBehaviour {

    public float minTargetChangeTime = 5.0f;
    public float maxTargetChangeTime = 10.0f;

    //--- Private References ---//
    private NavMeshAgent agent;



    //--- Private Variables ---//
    private float timeSinceLastTargetChange;
    private float timeUntilNextTargetChange;



    //--- Unity Functions ---//
    void Start()
    {
        //Init the private references
        agent = GetComponent<NavMeshAgent>();
        agent.destination = RandomNavSphere(transform.position, 100.0f, -1);

        //Init the private variables
        timeSinceLastTargetChange = 0.0f;
        timeUntilNextTargetChange = Random.Range(minTargetChangeTime, maxTargetChangeTime);
    }

    void Update()
    {
        //Increment the time since the last target change
        timeSinceLastTargetChange += Time.deltaTime;

        //If enough time has passed, change the target location
        if (timeSinceLastTargetChange >= timeUntilNextTargetChange)
        {
            //Reset the timer
            timeSinceLastTargetChange = 0.0f;

            //Change the target location
            agent.destination = RandomNavSphere(transform.position, 100.0f, -1);
        }
    }



    //--- Methods ---//
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        //Code found on:
        //https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}

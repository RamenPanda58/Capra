using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public float walkRadius = 2f;      // How far the agent can walk from its current position
    public float idleMin = 15f;         // Minimum idle time
    public float idleMax = 55f;         // Maximum idle time

    private bool isWalking = false;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        StartCoroutine(WalkIdleRoutine());
    }

    IEnumerator WalkIdleRoutine()
    {
        while (true)
        {
            // Choose a random point to walk to
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                isWalking = true;
                animator.SetBool("isWalking", true); // Make sure your Animator has "isWalking" parameter
            }

            // Wait until agent reaches destination
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            // Stop walking
            isWalking = false;
            animator.SetBool("isWalking", false);

            // Idle for random duration
            float idleTime = Random.Range(idleMin, idleMax);
            yield return new WaitForSeconds(idleTime);
        }
    }
}

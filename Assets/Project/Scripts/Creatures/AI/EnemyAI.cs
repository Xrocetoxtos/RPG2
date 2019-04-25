using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // General state machine variables
    private GameObject player;
    private Animator animator;
    private Ray ray;
    private RaycastHit hit;
    private float maxDistanceToCheck = 6.0f;
    private float currentDistance;
    private Vector3 checkDirection;

    public NavMeshAgent navMeshAgent;
    [SerializeField] private Transform[] waypoints;

    private int currentTarget=0; 
    private float distanceFromTarget;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }
    private void FixedUpdate()
    {
        //First we check distance from the player 
        currentDistance = Vector3.Distance(player.transform.position,transform.position);
        //animator.SetFloat("distanceFromPlayer", currentDistance);
        //Then we check for visibility
        checkDirection = player.transform.position - transform.position;
        ray = new Ray(transform.position, checkDirection);
        if (Physics.Raycast(ray, out hit, maxDistanceToCheck))
        {
            if (hit.collider.gameObject == player)
            {
                animator.SetBool("isPlayerVisible", true);
            }
            else
            {
                animator.SetBool("isPlayerVisible", false);
            }
        }
        else
        {
            animator.SetBool("isPlayerVisible", false);
        }
        //Lastly, we get the distance to the next waypoint target
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);
        //animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    }
    public void SetNextPoint()
    {
        currentTarget++;
        if (currentTarget > waypoints.Length - 1)
        {
            currentTarget = 0;
        }
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }
}
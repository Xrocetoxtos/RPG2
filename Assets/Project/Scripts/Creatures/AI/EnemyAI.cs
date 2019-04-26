﻿using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameHandler gameHandler;
    public NPC npc;

    private float aiTimer;

    private float IdleTimer;
    private float idleTimerStart = 2f;
    private float nothingTimer;
    private float nothingTimerStart = 10f;
    public float veryFar = 300;

    public Transform player;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Vector3 direction;
    private float angleToPlayer;
    private float distanceToPlayer;

    public bool npcLooksAtYou = false;
    public bool npcFollowsYou = false;
    public bool npcIsHostile = false;
    public bool npcIsArcher = false;

    public EnemyState npcState;
    private EnemyState lastState;
    private float alertness;

    [SerializeField] private int targetRange = 30;
    [SerializeField] private float viewAngle = 30f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = .2f;
    [SerializeField] private int walkTowardsRange = 20;
    [SerializeField] private int meleeRange = 2;
    [SerializeField] private int archeryRange = 30;

    [SerializeField] private Transform[] patrolArray;
    private Vector3 patrolPosition;
    private int patrolIndex = 0;
    private bool turningHead = false;   // voor omdraaien van plek naar plek
    public float patrolTurnSpeed = 2;

    public bool lookLeft = true;
    public float lookTotal = 0;
    public float turnHeadSpeed = 80f;

    private void Awake()
    {
        npc = GetComponent<NPC>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        npcState = EnemyState.Idle;
        IdleTimer = idleTimerStart;

        patrolPosition = patrolArray[patrolIndex].position;
        LocatePlayer();
    }

    private void Update()
    {
        npc.isInactive = false;
        switch (npcState)
        {
            case EnemyState.Nothing:
                alertness = 9999f;
                npc.isInactive = true;
                NPCNothing();
                break;
            case EnemyState.Idle:
                alertness = .5f;
                NPCIdle();
                LookForward();
                break;
            case EnemyState.Patrol:
                alertness = .2f;
                NPCPatrol();
                LookForward();
                break;
            case EnemyState.Busy:
                NPCBusy();
                LookForward();
                break;
            case EnemyState.Chase:
                NPCChase();
                break;
            case EnemyState.Attack:
                NPCAttack();
                break;
            case EnemyState.Search:
                NPCSearch();
                LookForward();
                break;
            default:
                break;
        }
    }



    // ====================================================================================
    //                                  States
    // ====================================================================================


    private void NPCNothing()
    {
        if (nothingTimer < 0f)
        {
            npcState = lastState;
        }
        else
        {
            nothingTimer -= Time.deltaTime;
        }
    }

    private void NPCIdle()
    {
        if (patrolArray.Length != 0)
        {
            //X-aantal seconden idle zijn en dan Patrol
            if (IdleTimer > 0)
            {
                IdleTimer -= Time.deltaTime;
            }
            else
            {
                IdleTimer = idleTimerStart;
                npcState = EnemyState.Patrol;
                turningHead = true;
                return;
            }
        }

        //hoofd van links naar rechts bewegen en zo zicht beïnvloeden
        MoveHead();
    }

    private void NPCPatrol()
    {
        patrolPosition = patrolArray[patrolIndex].position;
        //ben ik er al?
        if (Vector3.Distance(transform.position, patrolPosition) < moveSpeed)
        {
            //nieuwe doellocatie uit array halen.
            patrolIndex++;
            if (patrolIndex == patrolArray.Length)
            {
                //als de hele array is afgerond, Idle
                npcState = EnemyState.Idle;
                patrolIndex = 0;
                return;
            }
            turningHead = true;
            patrolPosition = patrolArray[patrolIndex].position;
        }
        else if (turningHead)
        {
            Vector3 targetDir = patrolPosition - transform.position;

            // The step size is equal to speed times frame time.
            float step = patrolTurnSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            float angle = Vector3.Angle(targetDir, transform.forward);

            transform.rotation = Quaternion.LookRotation(newDir);

            if (angle < patrolTurnSpeed)
            {
                turningHead = false;
                return;
            }
        }
        else
        {
            //over een array transforms lopen 
            navMeshAgent.SetDestination(patrolPosition);
            //hoofd steeds van links naar rechts draaien zoals bij Commandos
            MoveHead();
        }

    }

    private void NPCBusy()
    {
        // een animatie dat ie iets doet
        // lage alertness
    }

    private void NPCChase()
    {
        //achter de player aan gaan en stoppen op een locatie die bij je past, mele, ranged of friendly
        if (SeesPlayer())
        {

        }
        else
        {
            npcState = EnemyState.Search;
        }

    }

    private void NPCAttack()
    {
        if (SeesPlayer())
        {
            switch (DecideMeleeArcher())
            {
                case "melee":
                    MeleeAttack();
                    break;
                case "archer":
                    ArcherAttack();
                    break;
                default:
                    FindOutWayToAttack();
                    break;
            }
        }
        else
        {
            npcState = EnemyState.Search;
        }
    }

    private void NPCSearch()
    {
        // nog kijken hoe. nu even terug op patrouille
        npcState = EnemyState.Patrol;
    }



    // ====================================================================================
    //                                   Acties
    // ====================================================================================



    private void ActiveEnemy()
    {
        npc.NPCSetActive(true);
    }

    private void PassiveEnemy()
    {
        npc.NPCSetActive(false);
    }

    private void NPCDies()
    {
        //enemy gegevens in de GUI opruimen
        PassiveEnemy();

        //spullen neerleggen.
        //journal heroverwegen
        //opruimen van het gameobject en verwijzingen.

    }

    private void LocatePlayer()
    {
        direction = player.position - this.transform.position;
        angleToPlayer = Vector3.Angle(direction, this.transform.forward);
        distanceToPlayer = Vector3.Distance(player.position, this.transform.position);

        // niks doen als je te ver weg bent
        if (distanceToPlayer > veryFar)
        {
            lastState = npcState;
            npcState = EnemyState.Nothing;
            nothingTimer = nothingTimerStart;
        }
    }

    private void LookForward()
    {
        //afhankelijk van de state wordt een alertness ingesteld en die bepaalt hoe vaak de NPC kijkt.
        if (aiTimer > alertness)
        {
            if (SeesPlayer())
            {
                ActOnVision();
            }
            else
            {
                aiTimer = 0f;
            }
        }
        aiTimer += Time.deltaTime;

    }

    private bool SeesPlayer()
    {
        LocatePlayer();

        // speler dichtbij
        if (distanceToPlayer < targetRange)
        {
            // speler in line of sight
            if (angleToPlayer < viewAngle)
            {
                //zicht niet geblokkeerd
                if (!Physics.Linecast(transform.position, player.position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ActOnVision()
    {
        Debug.Log("HALLOOOO");
        npc.NPCSetActive(true);
    }

    private void MoveHead()
    {
        float rotation = 0;
        // hoofd van links naar rechts bewegen zodat npc dingen ziet.
        if (lookLeft)
        {
            rotation -= turnHeadSpeed * Time.deltaTime;
        }
        else
        {
            rotation += turnHeadSpeed * Time.deltaTime;
        }
        transform.Rotate(Vector3.up, rotation);
        lookTotal += rotation;

        if (lookTotal > 30)
        {
            lookLeft = true;
        }
        if (lookTotal < -30)
        {
            lookLeft = false;
        }
    }

    private string DecideMeleeArcher()
    {
        //wordt "melee" als de melee-aanval kan plaatsvinden, archer als je kunt schieten, anders niks.
        return "melee";
    }

    private void MeleeAttack()
    {

    }

    private void ArcherAttack()
    {

    }

    private void FindOutWayToAttack()
    {
        // waar is de player? moet ik lopen, zoeken, is hij weg?
        if (SeesPlayer())
        {
            npcState = EnemyState.Chase;
        }
        else
        {
            npcState = EnemyState.Search;
        }
    }



    //// General state machine variables
    //private GameObject player;
    //private Animator animator;
    //private Ray ray;
    //private RaycastHit hit;
    //private float maxDistanceToCheck = 6.0f;
    //private float currentDistance;
    //private Vector3 checkDirection;

    //public NavMeshAgent navMeshAgent;
    //[SerializeField] private Transform[] waypoints;

    //private int currentTarget=0; 
    //private float distanceFromTarget;
    //private void Awake()
    //{
    //    player = GameObject.FindWithTag("Player");
    //    animator = gameObject.GetComponent<Animator>();
    //    navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    //    navMeshAgent.SetDestination(waypoints[currentTarget].position);
    //}
    //private void FixedUpdate()
    //{
    //    //First we check distance from the player 
    //    currentDistance = Vector3.Distance(player.transform.position,transform.position);
    //    //animator.SetFloat("distanceFromPlayer", currentDistance);
    //    //Then we check for visibility
    //    checkDirection = player.transform.position - transform.position;
    //    ray = new Ray(transform.position, checkDirection);
    //    if (Physics.Raycast(ray, out hit, maxDistanceToCheck))
    //    {
    //        if (hit.collider.gameObject == player)
    //        {
    //            animator.SetBool("isPlayerVisible", true);
    //        }
    //        else
    //        {
    //            animator.SetBool("isPlayerVisible", false);
    //        }
    //    }
    //    else
    //    {
    //        animator.SetBool("isPlayerVisible", false);
    //    }
    //    //Lastly, we get the distance to the next waypoint target
    //    distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);
    //    //animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    //}
    //public void SetNextPoint()
    //{
    //    currentTarget++;
    //    if (currentTarget > waypoints.Length - 1)
    //    {
    //        currentTarget = 0;
    //    }
    //    navMeshAgent.SetDestination(waypoints[currentTarget].position);
    //}
}
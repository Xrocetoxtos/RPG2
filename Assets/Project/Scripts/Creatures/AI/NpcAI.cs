using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NpcAI : MonoBehaviour
{
    public GameHandler gameHandler;
    public NPC npc;

    protected float aiTimer;
    protected float alertness;

    protected float IdleTimer;
    protected float idleTimerStart = 2f;
    protected float nothingTimer;
    protected float nothingTimerStart = 10f;
    protected float veryFar = 300;

    public Transform player;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    protected Vector3 direction;
    protected float angleToPlayer;
    public float distanceToPlayer;
    public float interactDistance;      // voor dialogs voor het venster verdwijnt.

    public NPCState npcState;
    public NPCState lastState;
    public Vector3 lastSeenPlayer;

    [SerializeField] protected Transform[] patrolArray;
    protected Vector3 patrolPosition;
    protected int patrolIndex = 0;
    protected bool turningHead = false;   // voor omdraaien van plek naar plek
    [SerializeField] private int targetRange = 30;
    [SerializeField] private float viewAngle = 30f;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float turnSpeed = .2f;
    [SerializeField] protected float patrolTurnSpeed = 2;

    protected bool lookLeft = true;
    protected float lookTotal = 0;
    protected float turnHeadSpeed = 80f;

    protected virtual void Awake()
    {
        npc = GetComponent<NPC>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        player = GameObject.Find("PlayerReference").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        npcState = NPCState.Idle;
        IdleTimer = idleTimerStart;

        patrolPosition = patrolArray[patrolIndex].position;
        LocatePlayer();
        npc.isInactive = false;
    }


    protected virtual void Update()
    {
        switch (npcState)
        {
            case NPCState.Nothing:
                alertness = 9999f;
                npc.isInactive = true;
                NPCNothing();
                break;
            case NPCState.Idle:
                alertness = .5f;
                NPCIdle();
                LookForward();
                break;
            case NPCState.Patrol:
                alertness = .2f;
                NPCPatrol();
                LookForward();
                break;
            case NPCState.Busy:
                alertness = 1f;
                NPCBusy();
                LookForward();
                break;
            case NPCState.Flee:
                NPCFlee();
                break;
            default:
                break;
        }
    }

    // ====================================================================================
    //                                  States
    // ====================================================================================


    protected virtual void NPCNothing()
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

    protected virtual void NPCIdle()
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
                npcState = NPCState.Patrol;
                turningHead = true;
                return;
            }
        }
        //hoofd van links naar rechts bewegen en zo zicht beïnvloeden
        MoveHead();
    }

    protected virtual void NPCPatrol()
    {
        npc.NPCSetActive(false);
        patrolPosition = patrolArray[patrolIndex].position;
        //ben ik er al?
        if (Vector3.Distance(transform.position, patrolPosition) < moveSpeed)
        {
            //nieuwe doellocatie uit array halen.
            patrolIndex++;
            if (patrolIndex == patrolArray.Length)
            {
                //als de hele array is afgerond, Idle
                npcState = NPCState.Idle;
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

    protected virtual void NPCBusy()
    {
        navMeshAgent.SetDestination(transform.position);

    }

    protected virtual void NPCFlee()
    {

    }


    // ====================================================================================
    //                                   Acties
    // ====================================================================================


    protected virtual void NPCDies()
    {
        //enemy gegevens in de GUI opruimen
        npc.NPCSetActive(false);

        //spullen neerleggen.
        //journal heroverwegen
        //opruimen van het gameobject en verwijzingen.

    }

    protected void LookForward()
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

    protected virtual void ActOnVision()
    {

    }

    protected void MoveHead()
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

    protected virtual void FindNearestPatrolPoint()
    {
        float nearest = 0;
        int nearestPoint = 0;

        for (int i = 0; i < patrolArray.Length; i++)
        {
            float distanceToPoint = Vector3.Distance(patrolArray[i].position, transform.position);
            if (nearest > distanceToPoint || nearest == 0)
            {
                nearest = distanceToPoint;
                nearestPoint = i;
            }
        }

        if (nearest > 0)
        {
            npcState = NPCState.Patrol;
            patrolIndex = nearestPoint;
        }
        else
        {
            npcState = NPCState.Idle;
        }
    }


    // ====================================================================================
    //                                Bepalingen
    // ====================================================================================


    protected void LocatePlayer()
    {
        direction = player.position - this.transform.position;
        angleToPlayer = Vector3.Angle(direction, this.transform.forward);
        distanceToPlayer = Vector3.Distance(player.position, this.transform.position);

        // niks doen als je te ver weg bent
        if (distanceToPlayer > veryFar)
        {
            lastState = npcState;
            npcState = NPCState.Nothing;
            nothingTimer = nothingTimerStart;
        }
    }


    protected virtual bool SeesPlayer()
    {
        LocatePlayer();

        // speler dichtbij
        if (distanceToPlayer < targetRange)
        {
            // speler in line of sight
            if (angleToPlayer < viewAngle)
            {
                //zicht niet geblokkeerd, dus je kijkt direct naar Player
                RaycastHit hit;
                Debug.DrawLine(transform.position, player.position);
                if (Physics.Linecast(transform.position, player.position, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        lastSeenPlayer = player.position;
                        return true;
                    }
                }

            }
        }
        return false;
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : NpcAI
{
    public bool npcIsArcher = false;

    [SerializeField] private int walkTowardsRange = 20;
    [SerializeField] private int meleeRange = 2;
    [SerializeField] private int archeryRange = 30;

    protected override void Awake()
    {
        base.Awake();
        lastSeenPlayer = Vector3.up*10;
    }

    protected override void Update()
    {
        base.Update();
        npc.isInactive = false;
        switch (npcState)
        {
            case NPCState.Chase:
                NPCChase();
                break;
            case NPCState.Attack:
                NPCAttack();
                break;
            case NPCState.Search:
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


    protected override void NPCNothing()
    {
        base.NPCNothing();
        npc.NPCSetActive(false);
    }

    protected override void NPCIdle()
    {
        base.NPCIdle();
        npc.NPCSetActive(false);
    }

    protected override void NPCPatrol()
    {
        base.NPCPatrol();
        npc.NPCSetActive(false);
    }

    protected override void NPCBusy()
    {
        base.NPCBusy();
        npc.NPCSetActive(false);
    }

    private void NPCChase()
    {
        npc.NPCSetActive(true);
        //achter de player aan gaan en stoppen op een locatie die bij je past, mele, ranged of friendly
        if (SeesPlayer())
        {
            if(!PlayerInAttackRange())
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                npcState = NPCState.Attack;
            }
        }
        else
        {
            npcState = NPCState.Search;
        }

    }

    private void NPCAttack()
    {
        npc.NPCSetActive(true);
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
                    npcState = FindOutWayToAttack();
                    break;
            }
        }
        else
        {
            npcState = NPCState.Search;
        }
    }

    private void NPCSearch()
    {
        npc.NPCSetActive(true);
        float distance = Vector3.Distance(lastSeenPlayer, transform.position);

        if (Vector3.Distance(transform.position, lastSeenPlayer) < moveSpeed)
        {
            //Vanaf laatste positie player rondkijken
            LookAround();
            SeesPlayer();
        }
        else
        {
            //lopen naar laatste plek waar player gezien is.
            navMeshAgent.SetDestination(lastSeenPlayer);
            MoveHead();
        }

    }


    // ====================================================================================
    //                                   Acties
    // ====================================================================================


    protected override void NPCDies()
    {
        base.NPCDies();
    }


    protected override void ActOnVision()
    {
        base.ActOnVision();
        npc.NPCSetActive(true);
        lastSeenPlayer = player.position;
        npcState = DecideToAttack();
    }


    private void LookAround()
    {
        float rotation = 0;
        rotation += turnHeadSpeed * Time.deltaTime;
        
        transform.Rotate(Vector3.up, rotation);
        lookTotal += rotation;

        if (lookTotal > 360)
        {
            FindNearestPatrolPoint();
            lastSeenPlayer = Vector3.up*10;
        }
    }


    protected override void FindNearestPatrolPoint()
    {
        base.FindNearestPatrolPoint();
    }

    private NPCState DecideToAttack()
    {
        return NPCState.Chase;
    }

    private void MeleeAttack()
    {

    }

    private void ArcherAttack()
    {

    }



    // ====================================================================================
    //                                Bepalingen
    // ====================================================================================


    private string DecideMeleeArcher()
    {
        //wordt "melee" als de melee-aanval kan plaatsvinden, archer als je kunt schieten, anders niks.
        return "melee";
    }

    private float DecideAttackRange()
    {
        switch (DecideMeleeArcher())
        {
            case "melee":
                return meleeRange;
            case "archer":
                return archeryRange;
            default:
                return meleeRange;
        }
    }

    private bool PlayerInAttackRange()
    {
        return distanceToPlayer < DecideAttackRange();
    }

    private NPCState FindOutWayToAttack()
    {
        // waar is de player? moet ik lopen, zoeken, is hij weg?
        if (SeesPlayer())
        {
            return NPCState.Chase;
        }
        else
        {
            return NPCState.Search;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : NpcAI
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
        npc.isInactive = false;
        switch (npcState)
        {
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
                NPCBusy();
                LookForward();
                break;
            default:
                break;
        }
    }
}

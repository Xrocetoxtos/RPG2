using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    public NPC npc;

    private void Awake()
    {
        npc = GetComponent<NPC>();
        npc.isInactive = false;
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();
    [SerializeField] private Dictionary<NPC, int> popularityWithNPC = new Dictionary<NPC, int>();

    // vergelijkbare structuur met andere dingen (interacties met mensen, hints, etc)

    //Quest handling
    //==============
    public bool HasQuest(Quest quest)
    {
        return (questList.Contains(quest));
    }

    public Quest GetQuest(Quest quest)
    {
        if (HasQuest(quest))
        {
            return quest;
        }
        return null;
    }

    public void AddQuest(Quest quest)
    {
        if (!HasQuest(quest))
        {
            questList.Add(quest);
        }
    }

    public void RemoveQuest(Quest quest)
    {
        if (HasQuest(quest))
        {
            questList.Remove(quest);
        }
    }

    // Popularityhandling
    //===================
    public bool HasPopularity(NPC npc)
    {
        return popularityWithNPC.ContainsKey(npc);
    }

    public int GetPopularity(NPC npc)
    {
        return popularityWithNPC[npc];
    }

    public void AddPopularity(NPC npc, int pop)
    {
        //negatieve waarden zijn hier ook.
        if (!HasPopularity(npc))
        {
            popularityWithNPC.Add(npc, pop);
        }
        else
        {
            popularityWithNPC[npc] += pop;
        }
    }
}
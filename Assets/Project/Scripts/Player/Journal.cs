using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();
    [SerializeField] private Dictionary<NPC, int> popularityWithNPC = new Dictionary<NPC, int>();
    [SerializeField] private List<string> journalEntries = new List<string>();

    // vergelijkbare structuur met andere dingen (interacties met mensen, hints, etc)

    public void InsertEntry(string entry)
    {
        journalEntries.Add(entry);
    }

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

    public void AddQuest(Quest quest, NpcAI npcAI=null)
    {
        if (!HasQuest(quest))
        {
            questList.Add(quest);
            string entry = "I embarked upon a new quest ";
            if(npcAI!=null)
            {
                entry += "after talking to " + npcAI.gameObject.GetComponent<WorldObject>().objectTitle + ". ";
            }
            entry += quest.questDescription;
            InsertEntry(entry);
        }
    }

    public void RemoveQuest(Quest quest)
    {
        if (HasQuest(quest))
        {
            questList.Remove(quest);
        }
    }

    public void CheckAllActiveObjectives()
    {
        foreach(Quest q in questList)
        {
            if (q.questStatus!=QuestStatus.Completed && q.questStatus != QuestStatus.Failed)
            {
                int objectives = q.questObjectives.Length;
                int completedObjectives = 0;
                foreach (QuestObjective o in q.questObjectives)
                {
                    o.CheckObjectiveCompleted();
                    if(o.objectiveStatus == ObjectiveStatus.Completed)
                    {
                        completedObjectives++;
                    }
                }
                if (objectives == completedObjectives)
                {
                    Debug.Log("Alle objectives voltooid");
                    q.questStatus = QuestStatus.Successful;
                }
            }
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
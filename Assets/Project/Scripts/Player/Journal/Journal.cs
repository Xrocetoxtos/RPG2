using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    /*
     * TODO: 
     * quests en reputatie moeten ervoor zorgen dat de display opnieuw wordt opgebouwd. 
     * - entries tonen: alles
     * - quests tonen: alle quests en dan de entries en objectives rondom de quests rechts
     * - reputatie tonen: een visuele weergave van de reputatie per npc en de betreffende entries rechts
     */


    
    [SerializeField] private List<Quest> questList = new List<Quest>();
    public Dictionary<Quest, QuestGiver> questGivers = new Dictionary<Quest, QuestGiver>();
    [SerializeField] private Dictionary<NPC, int> popularityWithNPC = new Dictionary<NPC, int>();
    //    [SerializeField] private List<string> journalEntries = new List<string>();
    public List<JournalEntry> journalEntries = new List<JournalEntry>();
    private GameHandler gameHandler;
    private JournalDisplay display;


    [Header("Display")]
    public Sprite entrySprite;
    public Sprite questSprite;
    public Sprite reputationSprite;

    public Sprite emptyVinkje;
    public Sprite succesfullVinkje;
    public Sprite completedVinkje;
    public Sprite failedVinkje;

    public Sprite upArrow;
    public Sprite downArrow;
    private void Awake()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        display = GameObject.Find("JournalWindow").GetComponent<JournalDisplay>();
        display.Prime(journalEntries);
    }

    //Entries invoeren
    public void InsertEntry(JournalEntry entry)
    {
        journalEntries.Add(entry);
        display.PrimeEntry(entry);
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

    public void AddQuest(Quest quest, QuestGiver qg, NpcAI npcAI=null)
    {
        if (!HasQuest(quest))
        {
            quest.Init(qg);
            questList.Add(quest);
            questGivers.Add(quest, qg);
            InsertQuestStarted(quest, qg, npcAI);
        }
    }

    public void RemoveQuest(Quest quest)
    {
        if (HasQuest(quest))
        {
            questList.Remove(quest);
            questGivers.Remove(quest);

        }
    }

    public void CheckAllActiveObjectives(RelevantPosition rp=null)
    {
        foreach (Quest q in questList)
        {
            if (q.questStatus!=QuestStatus.Completed && q.questStatus != QuestStatus.Failed)
            {
                int objectives = q.questObjectives.Length;
                int completedObjectives = 0;
                foreach (QuestObjective o in q.questObjectives)
                {
                    o.CheckObjectiveCompleted(rp);
                    if(o.objectiveStatus == ObjectiveStatus.Completed)
                    {
                        completedObjectives++;
                    }
                }
                if (objectives == completedObjectives)
                {
                    q.questStatus = QuestStatus.Successful;
                    if(q.returnToGiver==false)
                    {
                        if (rp != null)
                        {
                            rp.busyFirstThing = true;
                        }
                        q.questGiver.CompletedQuest(q, q.questGiver, null);
                        q.questStatus = QuestStatus.Completed;
                        q.questGiver.InteractWithQuestGiver("questgiver", null, rp);
                        InsertQuestCompleted(q);
                    }
                }
            }
        }
    }

    public void InsertQuestStarted(Quest quest, QuestGiver qg, NpcAI npcAI = null)
    {
        string entryTxt = "I embarked upon a new quest";
        if (npcAI != null)
        {
            entryTxt += " after talking to " + npcAI.gameObject.GetComponent<WorldObject>().objectTitle;
        }
        entryTxt += ". ";
        entryTxt += quest.questDescription;
        JournalEntry entry = new JournalEntry(EntryTypes.Quest, entryTxt, quest, null);

        InsertEntry(entry);
    }

    public void InsertQuestCompleted(Quest quest, NpcAI npcAI = null)
    {
        //entry quest schrijven
        string entryTxt = "I finished the quest I got";
        if (npcAI != null)
        {
            entryTxt += " after talking to " + npcAI.gameObject.GetComponent<WorldObject>().objectTitle;
        }
        entryTxt += ". ";
        entryTxt += quest.questDescription;
        JournalEntry entry = new JournalEntry(EntryTypes.Quest, entryTxt, quest, null);
     
        InsertEntry(entry);

        //popularityWithNPC bijwerken
        AddPopularityDictionary(quest.popularityWithNPCCompleted);
        //nog een opvolgquest als die bestaat
        if (quest.nextQuest.Count > 0)
        {
            foreach (int q in quest.nextQuest)
            {
                Quest qu = gameHandler.GetQuestByID(q);
                if (qu != null)
                {
                    if (qu.questStatus == QuestStatus.Closed)
                    {
                        qu.questStatus = QuestStatus.Open;
                    }
                }
            }
        }
    }

    public void InsertQuestFailed(Quest quest, NpcAI npcAI = null)
    {
        //entry quest schrijven
        string entryTxt = "I failed the quest '" + quest.questTitle + "' that I got";
        if (npcAI != null)
        {
            entryTxt += " after talking to " + npcAI.gameObject.GetComponent<WorldObject>().objectTitle;
        }
        entryTxt += ". ";
        entryTxt += quest.questDescription;

        JournalEntry entry = new JournalEntry(EntryTypes.Quest, entryTxt, quest, null);
        InsertEntry(entry);

        //popularityWithNPC bijwerken
        AddPopularityDictionary(quest.popularityWithNPCFailed);
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
        string entryTxt;
        //negatieve waarden zijn hier ook.
        if (!HasPopularity(npc))
        {
            entryTxt = "My popularity with " + npc.gameObject.GetComponent<WorldObject>().objectTitle + " has been established at " + pop + ".";
            popularityWithNPC.Add(npc, pop);
        }
        else
        {
            entryTxt = "My popularity with " + npc.gameObject.GetComponent<WorldObject>().objectTitle + " has changed from " + popularityWithNPC[npc];
            popularityWithNPC[npc] += pop;
            entryTxt += " to " + popularityWithNPC[npc] + ".";
        }
        JournalEntry entry = new JournalEntry(EntryTypes.Reputation, entryTxt, null, npc, pop>0);

        InsertEntry(entry);

    }

    public void AddPopularityDictionary(Dictionary<NPC, int> dict)
    {
        if (dict.Count > 0)
        {
            foreach (KeyValuePair<NPC, int> npc in dict)
            {
                AddPopularity(npc.Key, npc.Value);
            }
        }
    }
}
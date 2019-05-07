using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestStatus questStatus;

    public int questID;
    public string questTitle;
    public string questDescription;

    // dialog-teksten voor iedere status van de quest.
    [Header("Dialog")]
    public string questOpenDialog;
    public string questPendingDialog;
    public string questActiveDialog;
    public string questSuccessfullDialog;
    public string questFailedDialog;
    public string questCompletedDialog;

    public string questActiveReply;
    public string questSuccessReply1;
    public string questSuccessReply2;
    public string questFailedReply;
    public string questCompletedReply;

    [Header("Objectives")]
    public QuestObjective[] questObjectives;
    public Dictionary<NPC, int> popularityWithNPCCompleted = new Dictionary<NPC, int>();
    public Dictionary<NPC, int> popularityWithNPCFailed = new Dictionary<NPC, int>();
    public NPC[] NPCCompleted;
    public int[] intCompleted;
    public NPC[] NPCFailed;
    public int[] intFailed;

    [Header("Reward")]
    public bool returnToGiver = true;           // als dit false is, is successful meteen completed
    public WorldObject[] rewardObjects;
    public int rewardCoins;
    public Quest[] nextQuest = null;            //quest die open gaat als je deze af hebt

    public void Init()
    {
        for (int i=0;i<NPCCompleted.Length;i++)
        {
            popularityWithNPCCompleted.Add(NPCCompleted[i], intCompleted[i]);
        }
        for (int i = 0; i < NPCFailed.Length; i++)
        {
            popularityWithNPCFailed.Add(NPCFailed[i], intFailed[i]);
        }
    }

    public bool QuestCompleted()
    {
        //alle objectives - meer dan 0 - moeten completed zijn
        if (questObjectives.Length > 0)
        {
            foreach (QuestObjective o in questObjectives)
            {
                if (o.objectiveStatus != ObjectiveStatus.Completed)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public string NameRewards()
    {
        string stuff = "";
        int counter = 0;
        bool objects = false;
        foreach (WorldObject obj in rewardObjects)
        {
            objects = true;
            counter++;
            if (counter == rewardObjects.Length && counter >1)
            {
                stuff += "and ";
            }
            stuff += obj.objectTitle;
            if(counter<rewardObjects.Length)
            {
                stuff += ", ";
            }
        }
        if (rewardCoins>0)
        {
            if (objects)
            {
                stuff += " and ";
            }
            stuff += rewardCoins.ToString() + " coins";
        }
        return stuff;
    }

    public string NameWhatYouGot(Inventory inventory)
    {
        string stuff = "";
        int counter = 0;
        bool objects = false;
        int coins = 0;
        List<WorldObject> inventoryObjects = new List<WorldObject>();
        foreach (WorldObject obj in rewardObjects)
        {
            if (inventory.GetItem(obj) != null)
            {
                inventoryObjects.Add(obj);
            }
        }

        foreach (WorldObject obj in inventoryObjects)
        {
            objects = true;
            counter++;
            if (counter == inventoryObjects.Count && counter > 1)
            {
                stuff += "and ";
            }
            stuff += obj.objectTitle;
            if (counter < inventoryObjects.Count)
            {
                stuff += ", ";
            }
        }
        if (inventory.creatureCoins > 0)
        {
            if (objects)
            {
                stuff += " and an amount of ";
            }
            if (rewardCoins < inventory.creatureCoins)
            {
                coins = rewardCoins;
            }
            else
            {
                coins= inventory.creatureCoins;
            }
            stuff += coins.ToString() + " coin";
            if(coins!=1)
            {
                stuff += "s";
            }
        }
        return stuff;
    }

    public void GiveWhatYouGot(Inventory giver, Inventory receiver)
    {
        foreach (WorldObject obj in rewardObjects)
        {
            if (giver.GetItem(obj) != null)
            {
                giver.GiveItem(receiver, obj);
            }
        }
        if (giver.creatureCoins < rewardCoins)
        {
            giver.GiveCoins(receiver, giver.creatureCoins);
        }
        else
        {
            giver.GiveCoins(receiver, rewardCoins);
        }
    }


}

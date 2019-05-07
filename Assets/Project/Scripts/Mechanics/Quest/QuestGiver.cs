using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public NpcAI npcAI;
    public Journal journal;

    public Inventory playerInventory;
    public Inventory giverInventory;

    public Quest quest;

    private GUIHandler guiHandler;
    private DialogHandler dialogHandler;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        journal = player.GetComponent<Journal>();
        playerInventory = player.GetComponent<Inventory>();
        guiHandler = GameObject.Find("GameHandler").GetComponent<GUIHandler>();
        dialogHandler = GameObject.Find("GameHandler").GetComponent<DialogHandler>();
        npcAI = GetComponent<NpcAI>();
        giverInventory = GetComponent<Inventory>();
    }

    public void InteractWithQuestGiver(string npc,NpcAI ai)
    {
        npcAI = ai;
        switch (quest.questStatus)
        {
            case QuestStatus.Closed:
                //als de quest closed is, niks tonen. de quest is niet beschikbaar.
                break;
            case QuestStatus.Open:
                QuestOpen(npc);
                break;
            case QuestStatus.Pending:
                QuestPending(npc);
                break;
            case QuestStatus.Active:
                QuestActive(npc);
                break;
            case QuestStatus.Successful:
                if (quest.returnToGiver)
                {
                    QuestSuccessful(npc);
                }
                else
                {
                    QuestCompleted(npc);
                }
                break;
            case QuestStatus.Failed:
                QuestFailed(npc);
                break;
            case QuestStatus.Completed:
                QuestCompleted(npc);
                break;
        }
    }

    public void CloseQuestWindow()
    {
        dialogHandler.ToggleDialog(false);
        npcAI.npcState = npcAI.lastState;
    }

    // ==================================================================
    //                            Statussen
    // ==================================================================

    private void QuestOpen(string npc)
    {
        string talk = quest.questOpenDialog;
        dialogHandler.ToggleDialog(true);
        if (npcAI != null)
        {
            talk += " I'll give you ";
        }
        else
        {
            talk += " You'll be rewarded with ";
        }
            
        talk+=  quest.NameRewards() + " in return.";
        dialogHandler.Talk(npcAI, quest.questTitle, talk, "Accept", "Decline",AcceptQuest, DeclineQuest, quest);
        quest.questStatus = QuestStatus.Pending;
    }

    private void QuestPending(string npc)
    {
        dialogHandler.ToggleDialog(true);
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questPendingDialog, "Accept", "Decline", AcceptQuest, DeclineQuest, quest);
    }

    private void QuestActive(string npc)
    {
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questActiveDialog, quest.questActiveReply, "", OkButton, dialogHandler.DoNothing, quest);
    }

    private void QuestSuccessful(string npc)
    {
        //als het een gather-quest-objective is, kun je kiezen het object niet te geven.
        //uitzoeken hoe dat te doen met meerdere objectives. als het kan wel dynamisch
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questSuccessfullDialog, quest.questSuccessReply1, quest.questSuccessReply2, SuccessQuest1, SuccessQuest2, quest);

    }

    private void QuestFailed(string npc)
    {
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questFailedDialog, quest.questFailedReply, "", FailedQuest, dialogHandler.DoNothing, quest);
    }

    private void QuestCompleted(string npc)
    {
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questCompletedDialog, quest.questCompletedReply, "", CompletedQuest, dialogHandler.DoNothing, quest);
    }

    // ==================================================================
    //                           Buttonfuncties
    // ==================================================================


    public void AcceptQuest(Quest quest, NpcAI npcai)
    {
        quest.questStatus = QuestStatus.Active;
        journal.AddQuest(quest, npcAI);
        CloseQuestWindow();

        int ob = 0;
        int oc = quest.questObjectives.Length;
        foreach (QuestObjective qo in quest.questObjectives)
        {
            qo.CheckObjectiveCompleted();
            if(qo.objectiveStatus == ObjectiveStatus.Completed)
            {
                ob++;
            }
        }
        if(ob==oc)
        {
            quest.questStatus = QuestStatus.Successful;
        }
    }

    public void DeclineQuest(Quest quest, NpcAI npcai)
    {
        quest.questStatus = QuestStatus.Open;
        CloseQuestWindow();
    }

    public void SuccessQuest1 (Quest quest, NpcAI npcai)
    {
        //journal bijwerken
        journal.InsertQuestCompleted(quest, npcai);

        if (giverInventory.EnoughInventory(quest.rewardCoins, quest.rewardObjects))
        {
            quest.questStatus = QuestStatus.Completed;

            //beloning
            giverInventory.GiveCoins(playerInventory,quest.rewardCoins);
            foreach (WorldObject item in quest.rewardObjects)
            {
                giverInventory.GiveItem(playerInventory, item);
            }

            // gather object verwijderen en naar npc
            foreach (QuestObjective obj in quest.questObjectives)
            {
                playerInventory.GiveItem(giverInventory, obj.objectToGather);
            }
            CloseQuestWindow();
        }
        else
        {
            //questgiver heeft niet alles als reward
            string npcTalk = "I'm sorry. It seems I don't have everything I promised you. While I promised you " + quest.NameRewards();
            npcTalk += ", I can only deliver " + quest.NameWhatYouGot(giverInventory) + ". Will you accept that as payment for your trouble?";
            dialogHandler.Talk(npcAI, quest.questTitle, npcTalk, "That'll do.", "No way!", SuccessQuest1a, SuccessQuest1b, quest);
        }
    }

    public void SuccessQuest1a(Quest quest, NpcAI npcai)
    {
        quest.GiveWhatYouGot(giverInventory, playerInventory);
            foreach (QuestObjective obj in quest.questObjectives)
            {
                playerInventory.GiveItem(giverInventory, obj.objectToGather);
            }
        CloseQuestWindow();
    }

    public void SuccessQuest1b(Quest quest, NpcAI npcai)
    {
        CloseQuestWindow();
    }

    public void SuccessQuest2(Quest quest, NpcAI npcai)
    {
        CloseQuestWindow();
        quest.questStatus = QuestStatus.Failed;
    }

    public void FailedQuest(Quest quest, NpcAI npcai)
    {

    }

    public void CompletedQuest(Quest quest, NpcAI npcai)
    {
        //nog een opvolgquest als die bestaat
        if (quest.nextQuest.Length >0)
        {
            foreach (Quest q in quest.nextQuest)
            {
                if (q.questStatus == QuestStatus.Closed)
                {
                    q.questStatus = QuestStatus.Open;
                }
            }
        }
        CloseQuestWindow();
    }

    public void OkButton (Quest quest, NpcAI npcai)
    {
        CloseQuestWindow();
    }

}

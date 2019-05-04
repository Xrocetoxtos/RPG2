using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public NpcAI npcAI;
    public Journal journal;
    public Quest quest;

    private GUIHandler guiHandler;
    private DialogHandler dialogHandler;

    private void Awake()
    {
        journal = GameObject.Find("Player").GetComponent<Journal>();
        guiHandler = GameObject.Find("GameHandler").GetComponent<GUIHandler>();
        dialogHandler = GameObject.Find("GameHandler").GetComponent<DialogHandler>();
        npcAI = GetComponent<NpcAI>();
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
                QuestSuccessful(npc);
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
        //Time.timeScale = 1;
    }

    // ==================================================================
    //                            Statussen
    // ==================================================================

    private void QuestOpen(string npc)
    {
        dialogHandler.ToggleDialog(true);
        dialogHandler.Talk(npcAI, quest.questTitle, quest.questOpenDialog, "Accept", "Decline",AcceptQuest, DeclineQuest, quest);
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
    }

    public void DeclineQuest(Quest quest, NpcAI npcai)
    {
        quest.questStatus = QuestStatus.Open;
        CloseQuestWindow();
    }

    public void SuccessQuest1 (Quest quest, NpcAI npcai)
    {

    }

    public void SuccessQuest2(Quest quest, NpcAI npcai)
    {

    }

    public void FailedQuest(Quest quest, NpcAI npcai)
    {

    }

    public void CompletedQuest(Quest quest, NpcAI npcai)
    {

    }

    public void OkButton (Quest quest, NpcAI npcai)
    {
        CloseQuestWindow();
    }

}

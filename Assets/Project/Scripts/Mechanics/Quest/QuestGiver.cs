using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public Journal journal;
    private GUIHandler guiHandler;
    private DialogHandler dialogHandler;

    private void Awake()
    {
        journal = GameObject.Find("Player").GetComponent<Journal>();
        guiHandler = GameObject.Find("GameHandler").GetComponent<GUIHandler>();
        dialogHandler = GameObject.Find("GameHandler").GetComponent<DialogHandler>();
    }

    public void InteractWithQuestGiver(string npc)
    {
        switch (quest.questStatus)
        {
            case QuestStatus.Closed:
                //als de quest closed is, niks tonen. de quest is nog niet beschikbaar.
                break;
            case QuestStatus.Open:
                QuestOpen(npc);
                break;
            case QuestStatus.Pending:
                QuestPending();
                break;
            case QuestStatus.Active:
                QuestActive();
                break;
            case QuestStatus.Successful:
                QuestSuccessful();
                break;
            case QuestStatus.Failed:
                QuestFailed();
                break;
            case QuestStatus.Completed:
                QuestCompleted();
                break;
        }
    }

    public void CloseQuestWindow()
    {
        guiHandler.dialogWindow.SetActive(false);
    }

    // ==================================================================

    private void QuestOpen(string npc)
    {
        dialogHandler.ToggleDialog(true);
        dialogHandler.Talk(npc, quest.questTitle, quest.questOpenDialog, "Accept", "Decline");
    }

    private void QuestPending()
    {

    }

    private void QuestActive()
    {

    }

    private void QuestSuccessful()
    {

    }

    private void QuestFailed()
    {

    }

    private void QuestCompleted()
    {

    }
}

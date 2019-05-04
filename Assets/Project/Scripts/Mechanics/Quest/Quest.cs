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
}

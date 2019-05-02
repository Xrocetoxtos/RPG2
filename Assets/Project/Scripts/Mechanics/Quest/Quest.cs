using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestStatus questStatus;
    public string questTitle;
    public string questDescription;

    // dialog-teksten voor iedere status van de quest.
    public string questOpenDialog;
    public string questPendingDialog;
    public string questActiveDialog;
    public string questSuccesfullDialog;
    public string questFailedDialog;
    public string questCompletedDialog;


}

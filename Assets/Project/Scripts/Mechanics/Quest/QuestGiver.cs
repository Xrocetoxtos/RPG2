using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public Journal journal;
    private GUIHandler guiHandler;
    

    private void Awake()
    {
        journal = GameObject.Find("Player").GetComponent<Journal>();
    }

    public void OpenQuestWindow()
    {
        guiHandler.questWindow.SetActive(true);
        guiHandler.ViewGUImessage(guiHandler.questTitle, quest.questTitle);
        guiHandler.ViewGUImessage(guiHandler.questDescription, quest.questDescription);
    }

    public void CloseQuestWindow()
    {
        guiHandler.questWindow.SetActive(false);
    }
}

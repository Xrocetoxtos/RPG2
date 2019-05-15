using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    private GameHandler gameHandler;
    private GUIHandler guiHandler;
    [SerializeField]    private Button buttonClick1;
    [SerializeField]    private Button buttonClick2;

    //voor de acties uit de buttons onder de dialogwindow
    public delegate void FireButton(Quest quest, QuestGiver qg, NpcAI npcai);
    public FireButton PressButton1 = delegate { };
    public FireButton PressButton2 = delegate { };

    public NpcAI npcAI = null;
    public QuestGiver questGiver = null;
    public bool dialogActive = false;

    Quest questInDialog = null;

    private void Awake()
    {
        gameHandler = GetComponent<GameHandler>();
        guiHandler = GetComponent<GUIHandler>();
    }

    private void Update()
    {
        if (npcAI != null)
        {
            if (dialogActive && npcAI.distanceToPlayer > npcAI.interactDistance)
            {
                ToggleDialog(false);
                gameHandler.Paused();
                npcAI.npcState = npcAI.lastState;
            }
        }
    }
    public void ToggleDialog(bool dialog)
    {
        guiHandler.mainPanel.SetActive(!dialog);
        guiHandler.dialogWindow.SetActive(dialog);
        if (dialog)
        {
            gameHandler.UnlockCursor();
            Time.timeScale = 0;
        }
        else
        {
            gameHandler.LockCursor();
            Time.timeScale = 1;
        }
        questInDialog = null;
        dialogActive = dialog;
    }

    public void Talk(NpcAI npc, string title, string text, string button1, string button2, FireButton fb1, FireButton fb2, Quest quest = null, QuestGiver giver=null)
    {
        questGiver = giver;
        // generieke manier om dialoog op te zetten. berichten en worden gepassed naar deze method
        Time.timeScale = 0;
        npcAI = npc;
        ToggleDialog(true);
        if (npc != null)
        {
            guiHandler.ViewGUImessage(guiHandler.dialogPartner, npc.gameObject.GetComponent<WorldObject>().objectTitle);
        }
        else
        {
            guiHandler.ViewGUImessage(guiHandler.dialogPartner, "Object");
        }
        guiHandler.ViewGUImessage(guiHandler.dialogTitle, title);
        guiHandler.ViewGUImessage(guiHandler.dialogDescription, text);
        guiHandler.ViewGUImessage(guiHandler.dialogButton1, button1);
        guiHandler.ViewGUImessage(guiHandler.dialogButton2, button2);

        PressButton1 = fb1;
        PressButton2 = fb2;
        questInDialog = quest;

        buttonClick1.gameObject.SetActive(true);
        buttonClick2.gameObject.SetActive(true); 
        if (button1 == "") buttonClick1.gameObject.SetActive(false);
        if (button2 == "") buttonClick2.gameObject.SetActive(false);
    }

    public void DoNothing(Quest q, QuestGiver qg, NpcAI n)
    {
        //als standaardfunctie om niks te doen nav een knop.
        ToggleDialog(false);
    }

    public void Press1()
    {
        PressButton1(questInDialog,questGiver,npcAI);
    }

    public void Press2()
    {
        PressButton2(questInDialog, questGiver, npcAI);
    }
}

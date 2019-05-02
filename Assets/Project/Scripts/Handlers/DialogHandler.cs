using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    GUIHandler guiHandler;

    private void Awake()
    {
        guiHandler = GetComponent<GUIHandler>();
    }

    public void ToggleDialog(bool dialog)
    {
        guiHandler.mainPanel.SetActive(!dialog);
        guiHandler.dialogWindow.SetActive(dialog);
    }

    public void Talk(string npc, string title, string text, string button1, string button2)
    {
        // generieke manier om dialoog op te zetten. berichten en worden gepassed naar deze method
        ToggleDialog(true);
        guiHandler.ViewGUImessage(guiHandler.dialogPartner, npc);
        guiHandler.ViewGUImessage(guiHandler.dialogTitle, title);
        guiHandler.ViewGUImessage(guiHandler.dialogDescription, text);
        guiHandler.ViewGUImessage(guiHandler.dialogButton1, button1);
        guiHandler.ViewGUImessage(guiHandler.dialogButton2, button2);
    }
}

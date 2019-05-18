using UnityEngine;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;
    public GUIHandler guiHandler;

    public GameObject player;
    private PlayerState playerState;
    public PlayerLook playerLook;

    public NPC currentEnemy = null;

    public bool isPaused = false;

    public List<Quest> allQuests = new List<Quest>();

    private void Awake()
    {
        guiHandler = GetComponent<GUIHandler>();
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitReferences();
        LockCursor();
    }

    private void InitReferences()
    {
        if (player == null)
        {
            guiHandler = GetComponent<GUIHandler>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerState = player.GetComponent<PlayerState>();
            playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
        {
            Paused();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryWindow();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            QuestWindow();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            JournalWindow();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            EquipmentWindow();
        }
    }

    private void InventoryWindow()
    {
        Paused(false);
        guiHandler.InventoryView();
    }

    private void JournalWindow()
    {

    }

    private void QuestWindow()
    {

    }

    private void EquipmentWindow()
    {

    }

    // Game States
    public void Paused(bool GUI=true)
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            guiHandler.ViewBothGUIMessages("", "");
        }
        else
        {
            if (GUI)
            {
                guiHandler.ViewGUImessage(guiHandler.guiMessage, "Game paused.");
                guiHandler.ViewGUImessage(guiHandler.guiMessage2, "");
            }
            else
            {
                guiHandler.ViewBothGUIMessages("", "");
            }
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    //Quests
    public Quest GetQuestByID(int id)
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.questID == id)
            {
                return quest;
            }
        }
        return null;
    }
}
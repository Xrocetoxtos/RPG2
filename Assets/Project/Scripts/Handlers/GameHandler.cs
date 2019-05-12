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
    }

    // Game States
    public void Paused()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            guiHandler.ViewBothGUIMessages("", "");
        }
        else
        {
            guiHandler.ViewGUImessage(guiHandler.guiMessage, "Game paused.");
            guiHandler.ViewGUImessage(guiHandler.guiMessage2, "");
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
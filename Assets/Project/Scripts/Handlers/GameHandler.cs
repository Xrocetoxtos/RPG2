using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;
    public GUIHandler guiHandler;

    public GameObject player;
    private PlayerState playerState;
    public PlayerLook playerLook;

    public NPC currentEnemy = null;

    public bool isPaused = false;

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
        TestHealthBar();
    }

    private void TestHealthBar()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerState.healthSystem.DamageHealth(5);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerState.healthSystem.HealHealth(5);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerState.healthSystem.DamageEnergy(5);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerState.healthSystem.HealEnergy(5);
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
}
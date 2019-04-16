using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;

    public GameObject player;
    private PlayerState playerState;
    public Inventory playerInventory;

    [Header("SceneManagement")]
    public int currentDoorNumber;
    public GameObject[] doorArray;
    public Quaternion doorRotation;
    public PlayerLook playerLook;
    private int index;

    [Header("Data")]
    public bool isPaused = false;

    [Header("GUI")]
    public Image black;
    public TextMeshProUGUI guiMessage;
    public TextMeshProUGUI guiMessage2;
    private float guiTextTimer = 0f;
    private float guiTextTimerMax = 2f;

    [SerializeField] private Image deepWaterMask;

    private void Awake()
    {
        if(instance ==null)
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
            player = GameObject.FindGameObjectWithTag("Player");
            playerState = player.GetComponent<PlayerState>();
            playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();
        }

        if (doorArray == null || doorArray.Length == 0)
        {
            doorArray = GameObject.FindGameObjectsWithTag("DoorScene");
        }

        playerInventory = new Inventory();

        //GUI elementen
        black = GameObject.Find("BlackBackground").GetComponent<Image>();

        deepWaterMask = GameObject.Find("DeepWaterMask").GetComponent<Image>();
        deepWaterMask.enabled = false;

        guiMessage = GameObject.Find("GUIMessage").GetComponent<TextMeshProUGUI>();
        guiMessage.SetText("");
        guiMessage2 = GameObject.Find("GUIMessage2").GetComponent<TextMeshProUGUI>();
        guiMessage2.SetText("");
    }

    private void Update()
    {
        ReconsiderGUI();
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
            ViewBothGUIMessages("", "");
        }
        else
        {
            ViewGUImessage(guiMessage, "Game paused.");
            ViewGUImessage(guiMessage2, "");
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

    //GUI
    public void ViewBothGUIMessages(string text, string text2)
    {
        ViewGUImessage(guiMessage, text);
        ViewGUImessage(guiMessage2, text2);
    }

    public void ViewGUImessage(TextMeshProUGUI tekstveld, string text, float timerMax=3f)
    {
        tekstveld.SetText(text);

        guiTextTimer = 0f;
        guiTextTimerMax = timerMax;
    }

    private void ReconsiderGUI()
    {
        guiTextTimer += Time.deltaTime;
        if (guiTextTimer> guiTextTimerMax && !isPaused)
        {
            guiTextTimer = 0;
            ViewBothGUIMessages("", "");
        }
    }


    //Scenemanagement
    public void LoadScene(int passedDoorNumber, int passedSceneNumber)
    {
        currentDoorNumber = passedDoorNumber;

        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(passedSceneNumber);
    }

    private void OnLevelWasLoaded()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorArray = GameObject.FindGameObjectsWithTag("DoorScene");
        playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();
        black.enabled =true;

        for (int i=0; i<doorArray.Length; i++)
        {
            DoorScene doorScene = doorArray[i].GetComponent<DoorScene>();
            if (doorScene.doorNumber == currentDoorNumber)
            {
                //FunctionTimer.Create(() => PlayerInNewScene(i), 2f);
                PlayerInNewScene(i);
            }
        }
        black.enabled = false;
    }

    private void PlayerInNewScene(int index)
    {
        //player naar de locatie van de deur verplaatsen
        player.transform.position = doorArray[index].transform.position;
        playerLook.transform.position = doorArray[index].transform.position;
        playerLook.LookCameraExternal(instance.doorRotation);
    }
}

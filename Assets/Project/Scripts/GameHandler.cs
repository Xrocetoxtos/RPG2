using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;

    public GameObject player;
    private PlayerState playerState;

    [Header("SceneManagement")]
    public int currentDoorNumber;
    public GameObject[] doorArray;
    public Quaternion doorRotation;
    public PlayerLook playerLook;
    private int index;

    [Header("Data")]
    public bool isPaused = false;

    [Header("GUI")]

    //crosshair kan straks naar playerinteract. ook in awake.
    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite[] crosshairImage;
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

        if (player ==null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(doorArray==null ||doorArray.Length==0)
        {
            doorArray = GameObject.FindGameObjectsWithTag("DoorScene");
        }

        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshair.sprite = crosshairImage[0];

        deepWaterMask = GameObject.Find("DeepWaterMask").GetComponent<Image>();
        deepWaterMask.enabled = false;

        playerState = player.GetComponent<PlayerState>();
        playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();

        LockCursor();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Paused();
        }
    }

    public void Paused()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
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


        for (int i=0; i<doorArray.Length; i++)
        {
            DoorScene doorScene = doorArray[i].GetComponent<DoorScene>();
            if (doorScene.doorNumber == currentDoorNumber)
            {
                index = i;
                FunctionTimer.Create(() => PlayerInNewScene(), .1f);
            }
        }
    }

    private void PlayerInNewScene()
    {
        //player naar de locatie van de deur verplaatsen
        player.transform.position = doorArray[index].transform.position;
        playerLook.transform.position = doorArray[index].transform.position;
        playerLook.LookCameraExternal(instance.doorRotation);
    }
}

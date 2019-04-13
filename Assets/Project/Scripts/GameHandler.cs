using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;

    public GameObject player;
    private PlayerState playerState;
    public Transform currentTransform;

    [Header("SceneManagement")]
    public int currentDoorNumber;
    public GameObject[] doorArray;

    [Header("Data")]

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
        //playerLook = player.transform.Find("PlayerCamera").gameObject.GetComponent<PlayerLook>();

        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(int passedDoorNumber, int passedSceneNumber, Transform doorTransform)
    {
        currentDoorNumber = passedDoorNumber;
        currentTransform = doorTransform;

        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(passedSceneNumber);
    }

    private void OnLevelWasLoaded()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorArray = GameObject.FindGameObjectsWithTag("DoorScene");

        for (int i=0; i<doorArray.Length; i++)
        {
            DoorScene doorScene = doorArray[i].GetComponent<DoorScene>();
            if (doorScene.doorNumber == currentDoorNumber)
            {
                //player naar de locatie van de deur verplaatsen
                player.transform.position = doorArray[i].transform.position;
                player.transform.rotation = instance.currentTransform.rotation;
                currentDoorNumber = 0;
                return;
            }
        }
    }
}

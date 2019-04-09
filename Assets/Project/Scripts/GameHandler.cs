using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;
    public int currentDoorNumber;

    public GameObject player;
    public GameObject[] doorArray;

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

    public void LoadScene(int passedDoorNumber, int passedSceneNumber)
    {
        Debug.Log("player" + player.transform.position);
        currentDoorNumber = passedDoorNumber;

        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(passedSceneNumber);
    }

    private void OnLevelWasLoaded()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorArray = GameObject.FindGameObjectsWithTag("DoorScene");

        for (int i=0; i<doorArray.Length; i++)
        {
            if(doorArray[i].GetComponent<DoorScene>().doorNumber == currentDoorNumber)
            {
                Debug.Log(doorArray[i].transform.position);

                player.transform.position = doorArray[i].transform.position;

            }
        }
    }
}

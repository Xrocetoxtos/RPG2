using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScene : MonoBehaviour
{
    public int doorNumber;
    public int doorSceneNumber;


    private void OnTriggerStay()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameHandler.instance.LoadScene(doorNumber, doorSceneNumber);
        }
    }

}

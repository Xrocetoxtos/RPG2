using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool isRunning = false;
    public bool isCrouching = false;
    public GameObject standingPlayer;
    public GameObject crouchingPlayer;

    private void Awake()
    {

        
    }

    public void PlayerStanding()
    {
        isCrouching = false;
        SetActiveState();
    }

    public void PlayerCrouching()
    {
        isCrouching = true;
        SetActiveState();
    }

    private void SetActiveState()
    {
        standingPlayer.SetActive(!isCrouching);
        crouchingPlayer.SetActive(isCrouching);
    }
}


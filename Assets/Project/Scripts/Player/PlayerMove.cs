using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerState playerState;
    public float moveSpeed = 6f;
    public float backBrake = 0.67f;
    public float crouchBrake = .5f;
    public float sprintMultiplier = 2f;
    private float actualMoveSpeed;

    private float moveTranslation;
    private float moveStraffe;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        GetMoveInput();
        if (moveTranslation != 0f || moveStraffe != 0f)
        {
            actualMoveSpeed = GetMoveSpeed();
            MovePlayer();
        }
    }

    private void GetMoveInput()
    {
        moveTranslation = Input.GetAxis("Vertical");
        moveStraffe = Input.GetAxis("Horizontal");

        PlayerRun();
        PlayerCrouch();
        PlayerJump();
    }

    private void PlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerState.isRunning = true;
        }
        else
        {
            playerState.isRunning = false;
        }
    }

    private void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log(playerState.isCrouching);

            if (!playerState.isCrouching)
            {
                playerState.PlayerCrouching();
            }
            else
            {
                playerState.PlayerStanding();
            }
            Debug.Log(playerState.isCrouching);



        }
    }

    private void PlayerJump()
    {

    }

    private float GetMoveSpeed()
    {
        float speed = moveSpeed;

        if(playerState.isRunning)
        {
            speed *= sprintMultiplier;
        }

        if(moveTranslation<0f)
        {
            speed *= backBrake;
        }
        if(playerState.isCrouching)
        {
            speed *= crouchBrake;
        }

        return speed;
    }

    private void MovePlayer()
    {
        float frameSpeed = actualMoveSpeed * Time.deltaTime;
        transform.Translate(moveStraffe * frameSpeed, 0, moveTranslation * frameSpeed);
    }

}
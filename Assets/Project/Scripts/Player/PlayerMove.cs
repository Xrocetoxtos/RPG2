using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float backBrake = 0.67f;
    public float sprintMultiplier = 2f;
    private bool isRunning = false;
    private float actualMoveSpeed;

    private float moveTranslation;
    private float moveStraffe;

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private float GetMoveSpeed()
    {
        float speed = moveSpeed;

        if(isRunning)
        {
            speed *= sprintMultiplier;
        }

        if(moveTranslation<0f)
        {
            speed *= backBrake;
        }

        return speed;
    }

    private void MovePlayer()
    {
        float frameSpeed = actualMoveSpeed * Time.deltaTime;
        transform.Translate(moveStraffe * frameSpeed, 0, moveTranslation * frameSpeed);
    }

}
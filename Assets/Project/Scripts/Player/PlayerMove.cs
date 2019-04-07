using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;

    private float moveTranslation;
    private float moveStraffe;

    private void Update()
    {
        GetMoveInput();
        if (moveTranslation != 0f || moveStraffe != 0f)
        {
            moveSpeed = GetMoveSpeed();
            MovePlayer();
        }
    }

    private void GetMoveInput()
    {
        moveTranslation = Input.GetAxis("Vertical");
        moveStraffe = Input.GetAxis("Horizontal");
    }

    private float GetMoveSpeed()
    {
        return moveSpeed;
    }

    private void MovePlayer()
    {
        float frameSpeed = moveSpeed * Time.deltaTime;
        transform.Translate(moveStraffe * frameSpeed, 0, moveTranslation * frameSpeed);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Vector2 mouseLook;
    private Vector2 smoothV;

    public float sensitivity = 5f;
    public float smoothing = 2f;
    public float clampStanding = 90f;
    public float clampCrouching = 60f;
    public float clampValue = 90f;

    public GameObject player;
    private GameHandler gameHandler;

    private void Start()
    {
        player = this.transform.parent.gameObject;
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    }

    private void Update()
    {
        if (!gameHandler.isPaused)
        {
            GetLookInput();
            if (mouseLook != Vector2.zero)
            {
                LookCamera();
            }
        }
    }

    private void GetLookInput()
    {
        float scaling = sensitivity * smoothing;
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(scaling, scaling));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLook += smoothV;

        //voorkomen dat je op zijn kop over je heen of onmder je door kunt kijken
        mouseLook.y = Mathf.Clamp(mouseLook.y, -clampValue, clampValue);
    }

    private void LookCamera()
    {
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }

    public void LookCameraExternal(Quaternion look)
    {
        player = this.transform.parent.gameObject;

        transform.localRotation = look;
        player.transform.localRotation = look;
    }
}
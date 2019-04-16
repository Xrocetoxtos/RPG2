using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance = null;

    public bool isRunning = false;
    public bool isCrouching = false;
    [SerializeField] private KeyCode crouchKey;

    private CharacterController characterController;
    private Transform playerCamera;
    public PlayerLook playerLook;
    private Vector3 upDownCam = new Vector3(0f, .5f, 0f);
    private Vector3 upDownBody = new Vector3(0f, .2f, 0f);

    public HealthSystem healthSystem;
    public Inventory inventory = new Inventory();


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            characterController = GetComponent<CharacterController>();
            playerCamera = transform.Find("PlayerCamera");
            playerLook = playerCamera.gameObject.GetComponent<PlayerLook>();
            healthSystem = new HealthSystem(100, 100);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CrouchInput();
    }

    private void CrouchInput()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            if(isCrouching)
            {
                PlayerStanding();
            }
            else
            {
                PlayerCrouching();
            }
        }
    }

    public void PlayerStanding()
    {
        isCrouching = false;
        characterController.height = 2;
        playerCamera.position += upDownCam;
        transform.position += upDownBody;
        playerLook.clampValue = playerLook.clampStanding;
    }

    public void PlayerCrouching()
    {
        isCrouching = true;
        characterController.height = 1.4f;
        playerCamera.position -= upDownCam;
        transform.position -= upDownBody;
        playerLook.clampValue = playerLook.clampCrouching;
    }
}


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
    public HealthBar healthBar;
    public EnergyBar energyBar;
    public OxigenBar oxigenBar;

    public Inventory inventory;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            characterController = GetComponent<CharacterController>();
            playerCamera = transform.Find("PlayerCamera");
            playerLook = playerCamera.gameObject.GetComponent<PlayerLook>();
            inventory = GetComponent<Inventory>();
            healthSystem = new HealthSystem(100, 100);
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            healthBar.Setup(healthSystem);
            energyBar = GameObject.Find("FatigueBar").GetComponent<EnergyBar>();
            energyBar.Setup(healthSystem);
            oxigenBar = GameObject.Find("OxigenBar").GetComponent<OxigenBar>();
            oxigenBar.Setup(healthSystem);
            GameObject.Find("GameHandler").GetComponent<GUIHandler>().AboveWater();

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


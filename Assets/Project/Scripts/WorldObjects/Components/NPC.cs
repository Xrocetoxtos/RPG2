using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    //script bevat alleen gegevens over de NPC, geen gedrag.
    public WorldObject worldObject;
    public EnemyAI enemyAI=null;
    public FriendlyAI friendlyAI = null;
    public bool isInactive = false;
    private bool isEnemy = false;

    public GameHandler gameHandler;
    public GUIHandler guiHandler;

    public HealthSystem healthSystem;
    public Inventory inventory;

    private Transform quad;         // voor de minimap icon

    private void Awake()
    {
        GameObject gh = GameObject.Find("GameHandler");
        worldObject = GetComponent<WorldObject>();
        enemyAI = GetComponent<EnemyAI>();
        friendlyAI = GetComponent<FriendlyAI>();
        if (enemyAI !=null)
        {
            isEnemy = true;
        }

        gameHandler = gh.GetComponent<GameHandler>();
        guiHandler = gh.GetComponent<GUIHandler>();
        inventory = GetComponent<Inventory>();
        healthSystem = new HealthSystem(20, 20);

        NPCSetActive(false);
        MiniMapInit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            NPCSetActive(true);
        }
        if (!isInactive)
        {
            MiniMapControl();
        }
    }

    public void NPCSetActive(bool active)
    {
        guiHandler.enemyHealthCanvas.SetActive(active);

        if (active)
        {
            guiHandler.enemyHealth.Setup(healthSystem);
            guiHandler.enemyHealth.Setup(healthSystem);
            gameHandler.currentEnemy = this;
            guiHandler.ViewGUImessage(guiHandler.enemyNameText, worldObject.objectTitle, 99999);
        }
        else
        {
            gameHandler.currentEnemy = null;
        }
    }

    private void MiniMapInit()
    {
        quad = transform.Find("Minimap Icon");

        if (isEnemy)
        {
            //rood
            quad.GetComponent<Renderer>().material = guiHandler.redCircle;
        }
        else
        {
            //groen
            quad.GetComponent<Renderer>().material = guiHandler.greenCircle;
        }
    }


    private void MiniMapControl()
    {
        //de minimap gaat met een quad op 30 boven de npc en die draait NIET mee met de rotatie van de npc, anders wordt ie soms zijwaarts zichtbaar
        Vector3 quadPosition = transform.position + new Vector3(0, 70, 0);
        quad.position = quadPosition;
        quad.eulerAngles = new Vector3(90, 180, 0);
        quad.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}

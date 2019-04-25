using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    //script bevat alleen gegevens over de NPC, geen gedrag.

    public GameHandler gameHandler;
    public GameObject healthCanvas;

    public HealthSystem healthSystem;
    public HealthBar healthBar;
    public EnergyBar energyBar;
    
    public Inventory inventory;

    private void Awake()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        healthCanvas = GameObject.Find("HealthSystemEnemy");
        inventory = GetComponent<Inventory>();
        healthSystem = new HealthSystem(20, 20);
        healthBar = GameObject.Find("EnemyHealthBar").GetComponent<HealthBar>();
        energyBar = GameObject.Find("EnemyFatigueBar").GetComponent<EnergyBar>();
        NPCSetActive(false);
    }


    //dit anders aanpakken. krijg een error als ik twee npc's heb. via de gamehandler spelen dat er iemand actief is.
    //geldt ook voor healthbar en energybar
    public void NPCSetActive(bool active)
    {
        healthCanvas.SetActive(active);

        if (active)
        {
            healthBar.Setup(healthSystem);
            energyBar.Setup(healthSystem);
            gameHandler.currentEnemy = this;
        }
        else
        {
            gameHandler.currentEnemy = null;
        }
    }
}

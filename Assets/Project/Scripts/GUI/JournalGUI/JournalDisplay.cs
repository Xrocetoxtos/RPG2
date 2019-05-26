using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalDisplay : MonoBehaviour
{
    public GameObject entryPanel;
    public GameObject questPanel;
    public GameObject reputationPanel;

    public Transform panel;
    public Transform targetTransform;
    
    // Start is called before the first frame update
    private void Awake()
    {
        entryPanel = GameObject.Find("EntryPanel");
        questPanel = GameObject.Find("QuestPanel");
        reputationPanel = GameObject.Find("ReputationPanel");
    }

    // Update is called once per frame
    private void Update()
    {
        ButtonsForViews();
    }

    private void ButtonsForViews()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetEntries();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetQuests();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetReputation();
        }
    }

    //knoppen voor andere weergave
    public void GetEntries()
    {
        entryPanel.SetActive(true);
        questPanel.SetActive(false);
        reputationPanel.SetActive(false);
    }

    public void GetQuests()
    {
        entryPanel.SetActive(false);
        questPanel.SetActive(true);
        reputationPanel.SetActive(false);
    }

    public void GetReputation()
    {
        entryPanel.SetActive(false);
        questPanel.SetActive(false);
        reputationPanel.SetActive(true);
    }
}

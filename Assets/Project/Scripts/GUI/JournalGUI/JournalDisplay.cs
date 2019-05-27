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

    private Journal journal;
    public JournalEntryDisplay journalEntryDisplayPrefab;
    public JournalEntryDisplay selectedJournalEntry;
    private bool selectedPrimed = false;

    public Sprite entrySprite;
    public Sprite questSprite;
    public Sprite reputationSprite;

    public Sprite emptyVinkje;
    public Sprite succesfullVinkje;
    public Sprite completedVinkje;
    public Sprite failedVinkje;

    public Sprite upArrow;
    public Sprite downArrow;

    private void Awake()
    {
        entryPanel = GameObject.Find("EntryPanel");
        questPanel = GameObject.Find("QuestPanel");
        reputationPanel = GameObject.Find("ReputationPanel");
        journal = GameObject.Find("Player").GetComponent<Journal>();
    }

    private void Update()
    {
        ButtonsForViews();
    }

    public void Prime(List<JournalEntry> entries)
    {
        foreach(JournalEntry entry in entries)
        {
            if (selectedPrimed)
            {
                SelectEntry(entry);
            }
            PrimeEntry(entry);
        }
        if(selectedJournalEntry.entry==null)
        {
            selectedJournalEntry.Empty();
        }
        selectedPrimed = false;
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
        //entryPanel.SetActive(true);
        //questPanel.SetActive(false);
        //reputationPanel.SetActive(false);
    }

    public void GetQuests()
    {
        //entryPanel.SetActive(false);
        //questPanel.SetActive(true);
        //reputationPanel.SetActive(false);
    }

    public void GetReputation()
    {
        //entryPanel.SetActive(false);
        //questPanel.SetActive(false);
        //reputationPanel.SetActive(true);
    }

    public void PrimeEntry(JournalEntry entry)
    {
        JournalEntryDisplay display = (JournalEntryDisplay)Instantiate(journalEntryDisplayPrefab);
        display.transform.SetParent(targetTransform, false);
        display.Prime(entry);
    }

    public void SelectEntry(JournalEntry entry)
    {
        selectedJournalEntry.Prime(entry);
        selectedPrimed = true;
    }


}

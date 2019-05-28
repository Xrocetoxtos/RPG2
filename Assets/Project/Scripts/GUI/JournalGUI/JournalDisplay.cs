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
        if(selectedJournalEntry.entryDisplay==null)
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
        EmptyDisplay();
        PrimeOnTypes(journal.journalEntries,EntryTypes.Entry);
    }

    public void GetQuests()
    {
        EmptyDisplay();
        PrimeOnTypes(journal.journalEntries, EntryTypes.Quest);
    }

    public void GetReputation()
    {
        EmptyDisplay();
        PrimeOnTypes(journal.journalEntries, EntryTypes.Reputation);
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

    private void EmptyDisplay()
    {
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }
    }

    private void PrimeOnTypes(List<JournalEntry> entries, EntryTypes entryType)
    {
        selectedPrimed = false;
        foreach (JournalEntry entry in entries)
        {
            if (entry.entryType == entryType || entryType==EntryTypes.Entry)
            {
                if (!selectedPrimed)
                {
                    SelectEntry(entry);
                }
                PrimeEntry(entry);
            }
        }
        if (selectedJournalEntry.entryDisplay == null)
        {
            selectedJournalEntry.Empty();
        }
        selectedPrimed = false;
    }

}

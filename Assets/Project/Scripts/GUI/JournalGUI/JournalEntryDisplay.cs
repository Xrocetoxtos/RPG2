using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalEntryDisplay : MonoBehaviour
{
    public TextMeshProUGUI entryText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI objectiveText;
    public Image mainSprite;
    public Image statusSprite;

    public JournalDisplay journal;
    public GameObject objectivesObject;
    public JournalEntry entry;

    private void Awake()
    {
        journal = GameObject.Find("JournalWindow").GetComponent<JournalDisplay>();
        objectivesObject = GameObject.Find("QuestObjectives");
    }

    public void Prime(JournalEntry entry)
    {
        if (entryText!=null)
        {
            entryText.SetText(entry.entryText);
        }
        if (descriptionText!=null)
        {
            PrimeDescription(entry);
        }
        if (objectiveText != null)
        {
            PrimeObjectives(entry);
        }
        if (mainSprite != null)
        {
            PrimeMainSprite(entry);
        }
        if (statusSprite != null)
        {
            PrimeStatusSprite(entry);
        }
    }

    private void PrimeDescription(JournalEntry entry)
    {
        string desc = "";
        switch (entry.entryType)
        {
            case EntryTypes.Entry:
                {
                    desc += "I wrote the following in my journal, as I might have use of that later: \n\n";
                    desc += entry.entryText;
                    break;
                }
            case EntryTypes.Quest:
                {
                    desc += "I wrote down an update on the quest '" + entry.entryQuest.questTitle + "' for later reference: \n\n";
                    desc += entry.entryText + "/n/n More on the quest: /n";
                    desc += entry.entryQuest.questDescription;
                    break;
                }
            case EntryTypes.Reputation:
                {
                    desc += "Whatever I did, it had some influence on what " + entry.entryNpc.gameObject.GetComponent<WorldObject>().objectTitle + " thinks of me, so I wrote that down:  /n/n";
                    desc += entry.entryText;
                    break;
                }
        }
        descriptionText.SetText(desc);
    }

    private void PrimeObjectives(JournalEntry entry)
    {
        if (entry.entryType != EntryTypes.Quest)
        {
            objectivesObject.SetActive(false);
            return;
        }
        objectivesObject.SetActive(true);
    }

    private void PrimeMainSprite(JournalEntry entry)
    {
        switch (entry.entryType)
        {
            case EntryTypes.Entry:
                {
                    mainSprite.sprite=journal.entrySprite;
                    break;
                }
            case EntryTypes.Quest:
                {
                    mainSprite.sprite = journal.questSprite;
                    break;
                }
            case EntryTypes.Reputation:
                {
                    mainSprite.sprite = journal.reputationSprite;
                    break;
                }
        }
    }

    private void PrimeStatusSprite(JournalEntry entry)
    {
        switch (entry.entryType)
        {
            case EntryTypes.Entry:
                {
                    statusSprite.enabled = false;
                    break;
                }
            case EntryTypes.Quest:
                {
                    statusSprite.enabled = true;
                    if (entry.entryQuest.questStatus == QuestStatus.Active)
                        statusSprite.sprite = journal.emptyVinkje;
                    if (entry.entryQuest.questStatus == QuestStatus.Failed)
                        statusSprite.sprite = journal.failedVinkje;
                    if (entry.entryQuest.questStatus == QuestStatus.Successful)
                        statusSprite.sprite = journal.succesfullVinkje;
                    if (entry.entryQuest.questStatus == QuestStatus.Completed)
                        statusSprite.sprite = journal.completedVinkje;
                    break;
                }
            case EntryTypes.Reputation:
                {
                    statusSprite.enabled = true;
                    if (entry.entryUpwards)
                        statusSprite.sprite = journal.upArrow;
                    else
                        statusSprite.sprite = journal.downArrow;
                    break;
                }
        }
    }

    public void SelectEntry()
    {

    }

    public void Empty()
    {
        if (entryText != null)
        {
            entryText.SetText("");
        }
        if (descriptionText != null)
        {
            descriptionText.SetText("No entries found.");
        }
        if (objectiveText != null)
        {
            objectivesObject.SetActive(false);
        }
        if (mainSprite != null)
        {
            mainSprite.enabled = false;
        }
        if (statusSprite != null)
        {
            statusSprite.enabled = false;
        }
    }

    public void SelectButton()
    {
        journal.SelectEntry(entry);
    }
}

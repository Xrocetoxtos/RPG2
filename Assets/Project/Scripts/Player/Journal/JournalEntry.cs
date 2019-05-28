using UnityEngine;

[System.Serializable]
public class JournalEntry
{
    public EntryTypes entryType;
    public Quest entryQuest;
    public NPC entryNpc;
    public string entryText;
    public bool entryUpwards;

    public JournalEntry(EntryTypes type, string text, Quest quest = null, NPC npc = null, bool upwards=false)
    {
        entryType = type;
        entryText = text;
        entryQuest = quest;
        entryNpc = npc;
        entryUpwards = upwards;
    }
}

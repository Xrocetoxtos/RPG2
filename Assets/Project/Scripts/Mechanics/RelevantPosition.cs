using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelevantPosition : MonoBehaviour
{
    private QuestGiver questGiver=null;
    private Journal journal;

    private void Awake()
    {
        journal = GameObject.Find("Player").GetComponent<Journal>();
        questGiver = GetComponent<QuestGiver>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (questGiver != null)
        {
            if ((questGiver.quest.questStatus == QuestStatus.Open || questGiver.quest.questStatus == QuestStatus.Pending) && col.gameObject.tag == "Player")
            {
                questGiver.InteractWithQuestGiver("testcollider", null);
            }
        }
        journal.CheckAllActiveObjectives();
    }
}

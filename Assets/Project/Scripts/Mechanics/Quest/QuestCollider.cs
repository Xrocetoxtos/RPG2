using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollider : MonoBehaviour
{
    public QuestGiver questGiver;

    private void Awake()
    {
        questGiver = GetComponent<QuestGiver>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if((questGiver.quest.questStatus== QuestStatus.Open || questGiver.quest.questStatus == QuestStatus.Pending) && col.gameObject.tag =="Player")
        {
            questGiver.InteractWithQuestGiver("testcollider", null);
        }
    }
}

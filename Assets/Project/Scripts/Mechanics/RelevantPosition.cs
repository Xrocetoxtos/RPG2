using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelevantPosition : MonoBehaviour
{
    private QuestGiver questGiver=null;
    private Journal journal;

    public bool busyFirstThing = false;

    private void Awake()
    {
        journal = GameObject.Find("Player").GetComponent<Journal>();
        questGiver = GetComponent<QuestGiver>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //altijd eerst kijken of deze collision een quest oplost
            journal.CheckAllActiveObjectives(this);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (!busyFirstThing)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("ben er");
                if (questGiver != null)
                {
                    Debug.Log("ben er qg");

                    if ((questGiver.quest.questStatus == QuestStatus.Open || questGiver.quest.questStatus == QuestStatus.Pending))
                    {
                        Debug.Log("ben er q");

                        questGiver.InteractWithQuestGiver("testcollider", null, this);
                    }
                }
            }
        }
    }
}

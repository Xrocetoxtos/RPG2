using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    private GameObject player;
    private Inventory inventory;
    private Journal journal;
    
    public ObjectiveType objectiveType = ObjectiveType.Gather;
    public ObjectiveStatus objectiveStatus = ObjectiveStatus.Pending;

    public WorldObject objectToGather = null;
    public NpcAI enemyToDefeat = null;
    public RelevantPosition positionToMove=null;       //moet een object met een collider zijn.

    private void Awake()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<Inventory>();
        journal = player.GetComponent<Journal>();
    }

    public void CheckObjectiveCompleted(RelevantPosition rp=null)
    {
        switch (objectiveType)
        {
            case ObjectiveType.Gather:
                CheckGatherCompleted();
                break;
            case ObjectiveType.Defeat:
                CheckDefeatCompleted();
                break;
            case ObjectiveType.Move:
                CheckMoveCompleted(rp);
                break;
        }
    }

    private void CheckGatherCompleted()
    {
        if(inventory.GetItem(objectToGather) != null)
        {
            objectiveStatus = ObjectiveStatus.Completed;
        }
        else
        {
            objectiveStatus = ObjectiveStatus.Pending;
        }
    }

    private void CheckDefeatCompleted()
    {

    }

    private void CheckMoveCompleted(RelevantPosition rp=null)
    {
        if(positionToMove==rp)
        {
            objectiveStatus = ObjectiveStatus.Completed;
        }
        else
        {
            objectiveStatus = ObjectiveStatus.Pending;
        }
    }
}

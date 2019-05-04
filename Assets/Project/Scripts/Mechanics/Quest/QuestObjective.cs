using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    private GameObject player;
    private Inventory inventory;
    
    public ObjectiveType objectiveType = ObjectiveType.Gather;
    public ObjectiveStatus objectiveStatus = ObjectiveStatus.Pending;

    public WorldObject objectToGather = null;
    public NpcAI enemyToDefeat = null;
    public GameObject positionToMove=null;       //moet een empty met een collider zijn.

    private void Awake()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<Inventory>();
    }

    public void CheckObjectiveCompleted()
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
                CheckMoveCompleted();
                break;
        }
    }

    private void CheckGatherCompleted()
    {
        if(inventory.GetItem(objectToGather) != null)
        {
            objectiveStatus = ObjectiveStatus.Completed;
            Debug.Log("completed");
        }
        else
        {
            objectiveStatus = ObjectiveStatus.Pending;
            Debug.Log("pending");
        }
    }

    private void CheckDefeatCompleted()
    {

    }

    private void CheckMoveCompleted()
    {

    }
}

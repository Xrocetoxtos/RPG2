using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public ObjectiveType objectiveType = ObjectiveType.Gather;
    public ObjectiveStatus objectiveStatus = ObjectiveStatus.Pending;

    public WorldObject objectToGather = null;
    public int amountToGather = 0;
    public NpcAI enemyToDefeat = null;
    public GameObject positionToMove;       //moet een empty met een collider zijn.


}

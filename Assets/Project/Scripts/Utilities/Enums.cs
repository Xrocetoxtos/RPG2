public enum ObjectType
{
    Item,
    Interactable,
    Door,
    NPC
}

public enum NPCState
{
    Nothing,
    Idle,
    Busy,
    Patrol,
    Chase,
    Attack,
    Search,
    Flee
}

public enum QuestStatus
{
    Closed,
    Open,
    Pending,
    Active,
    Successful,
    Failed,
    Completed
}

public enum ObjectiveStatus
{
    Pending,
    Completed,
    Failed
}

public enum ObjectiveType
{
    Gather,
    Defeat,
    Move
}
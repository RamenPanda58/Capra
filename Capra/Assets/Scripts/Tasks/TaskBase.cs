using UnityEngine;

public abstract class TaskBase : ScriptableObject, ITask
{
    [SerializeField] private string taskName;
    [SerializeField] private string requiredItem;
    [SerializeField] private string rewardCode;
    [SerializeField] private string targetLocationName;

    public string TaskName => taskName;
    public string RequiredItem => requiredItem;
    public string RewardCode => rewardCode;
    public string TargetLocationName => targetLocationName;

    public abstract void StartTask();
    public abstract void PerformTask();
    public abstract void CompleteTask();

    public bool CanPerform(string heldItem, TaskLocation currentLocation)
    {
        if (currentLocation == null) return false;
        return heldItem == requiredItem && currentLocation.LocationName == targetLocationName;
    }
}

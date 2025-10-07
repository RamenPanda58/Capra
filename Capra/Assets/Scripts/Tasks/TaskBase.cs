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
    public bool IsComplete;
    bool ITask.IsComplete { get => IsComplete; set => IsComplete = value; }

    public abstract void StartTask();
    public abstract void PerformTask();
    public abstract void CompleteTask();

    public virtual bool CanPerform(TaskLocation currentLocation, string requiredItem)
    {
        return PlayerInventory.Instance.HasItem(requiredItem) && currentLocation.LocationName== targetLocationName;
    }
}

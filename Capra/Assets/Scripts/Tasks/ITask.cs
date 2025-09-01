using UnityEngine;

public interface ITask 
{
    string TaskName { get; }
    string RequiredItem { get; }

    void StartTask();
    void PerformTask();
    void CompleteTask();

}
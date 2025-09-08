using UnityEngine;

public interface ITask 
{
    string TaskName { get; }
    string RequiredItem { get; }

    bool IsComplete { get; set; }

    void StartTask();
    void PerformTask();
    void CompleteTask();

}
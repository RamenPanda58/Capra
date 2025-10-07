using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskBase currentTask;
    public TextMeshProUGUI CurrentQuest;

    static public List<TaskBase> completedTasks;

    public void Awake()
    {
        completedTasks = new List<TaskBase>();
    }

    public void StartTask(TaskBase task)
    {
        if (completedTasks.Contains(task)) return;
        currentTask = task;
        currentTask.StartTask();
        Debug.Log("Task started: " + (currentTask).TaskName);
        CurrentQuest.text = "You might want to " + (currentTask).TaskName;
    }

    public bool TryPerformTask()
    {
        if (currentTask == null)
        {
            Debug.Log("No active task.");
            return false;
        }

        TaskLocation currentLocation = TaskLocation.CurrentLocation;
        if (currentLocation == null)
        {
            Debug.Log("You are not at the correct location.");
            return false;
        }

        

        if (currentTask.CanPerform(currentLocation, currentTask.RequiredItem))
        {
            currentTask.PerformTask();
            return true;
        }
        else
        {
            Debug.Log("Cannot perform task: wrong item or location.");
            return false;
        }
    }
}

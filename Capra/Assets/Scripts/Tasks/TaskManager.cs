using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public static TaskManager Instance { get; private set; }

    public TaskBase CurrentTask { get; private set; }

    public TaskBase currentTask;
    public TextMeshProUGUI CurrentQuest;

    static public List<TaskBase> completedTasks;

    public void Awake()
    {
        completedTasks = new List<TaskBase>();
        Instance = this;
    }

    public void StartTask(TaskBase task)
    {
        if (completedTasks.Contains(task)) return;
        currentTask = task;
        currentTask.StartTask();
        Debug.Log("Task started: " + (currentTask).TaskName);
        CurrentQuest.text = "You might want to " + (currentTask).TaskName;
    }

    public void SetCurrentTask(TaskBase newTask)
    {
        CurrentTask = newTask;
        newTask.StartTask();
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

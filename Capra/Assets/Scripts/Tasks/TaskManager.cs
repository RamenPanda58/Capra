using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private TaskBase currentTask;

    public void StartTask(TaskBase task)
    {
        currentTask = task;
        currentTask.StartTask();
        Debug.Log("Task started: " + (currentTask).TaskName);
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

        string heldItem = PlayerInventory.Instance.GetHeldItem();

        if (currentTask.CanPerform(heldItem, currentLocation))
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

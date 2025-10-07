using HeneGames.DialogueSystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour, IInteractable
{
    [SerializeField] private TaskBase assignedTask;
    [SerializeField] private string rewardItemName = "Coin"; // Example reward
    [SerializeField] private int rewardAmount = 1;

    private TaskManager taskManager;
    private DialogueManager dialogueManager;

   
    private void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
        dialogueManager = GetComponentInChildren<DialogueManager>();
    }

    public void Interact()
    {
        // If player hasn’t started the task yet
        if (taskManager.currentTask == null && assignedTask != null)
        {
            taskManager.StartTask(assignedTask);
            assignedTask.StartTask();
            return;
        }

        // If task is complete
        if (assignedTask != null && assignedTask.IsComplete)
        {
            PlayerInventory.Instance.AddItem(rewardItemName, rewardAmount);

            WorldProgression.Instance.ApplyReward(rewardItemName);

            taskManager.currentTask = null;
        }
    }
}

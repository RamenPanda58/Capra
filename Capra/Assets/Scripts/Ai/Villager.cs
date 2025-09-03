using UnityEngine;

public class Villager : Interactable
{
    [SerializeField] private TaskBase taskToGive;
    [SerializeField] private string itemToGive;

    public override void Interact()
    {
        Debug.Log("Talking to villager...");
        FindFirstObjectByType<TaskManager>().StartTask(taskToGive);
        if (itemToGive != null) 
        {
            Debug.Log("Villager gave " +itemToGive+ " to the player.");
            PlayerInventory.Instance.AddItem(itemToGive);
        }
    }
}

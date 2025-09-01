using UnityEngine;

public class Villager : MonoBehaviour, IInteractable
{
    [SerializeField] private TaskBase taskToGive;
    [SerializeField] private string itemToGive;

    public void Interact()
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

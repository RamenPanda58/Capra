using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Weeding Task")]
public class Weeding : TaskBase
{
    [SerializeField] private int weedsRequired = 1;    // How much weed to collect
    [SerializeField] private string weedsItemName = "Weed"; // Item name to give player
    [SerializeField] private string toolRequired = "Sickle";  // Required item to perform task

    public override void StartTask()
    {
        Debug.Log($"Go to {TargetLocationName} with an {toolRequired}.");
        PlayerInventory.Instance.AddItem("Sickle");
        IsComplete = false;
    }

    public override void PerformTask()
    {
        if (IsComplete) return;
        string heldItem = PlayerInventory.Instance.GetHeldItem();

        if (!PlayerInventory.Instance.HasItem(toolRequired))
        {
            Debug.Log($"You need a {toolRequired} to get rid of the weeds!");
            return;
        }

        PlayerInventory.Instance.AddItem(weedsItemName, 1);
        int currentWood = PlayerInventory.Instance.GetItemCount(weedsItemName);

        Debug.Log($"You cut a {weedsItemName}. You now have {currentWood}/{weedsRequired}.");

        if (currentWood >= weedsRequired)
        {
            Debug.Log("You have collected enough weeds!");
            IsComplete = true;
            CompleteTask();
        }
    }

    public override void CompleteTask()
    {
        TaskManager.completedTasks.Add(this);
        WorldProgression.Instance.ApplyReward("WeedingFinished");
        PlayerInventory.Instance.RemoveItem("Sickle");
        Debug.Log("Weeding task completed!");
    }

}

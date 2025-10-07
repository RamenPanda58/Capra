using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Eggcollecting Task")]
public class EggCollectingTask : TaskBase
{
    [SerializeField] private int eggRequired = 1;    // How much egg to collect
    [SerializeField] private string eggItemName = "Egg"; // Item name to give player
    [SerializeField] private string toolRequired = "Basket";  // Required item to perform task

    public override void StartTask()
    {
        Debug.Log($"Go to {TargetLocationName} with a {toolRequired}.");
        PlayerInventory.Instance.AddItem("Basket");
        IsComplete = false;
    }

    public override void PerformTask()
    {
        if (IsComplete) return;
        string heldItem = PlayerInventory.Instance.GetHeldItem();

        if (!PlayerInventory.Instance.HasItem("Basket"))
        {
            Debug.Log($"You need a {toolRequired} to collect eggs!");
            return;
        }

        PlayerInventory.Instance.AddItem(eggItemName, 1);
        int currentEgg = PlayerInventory.Instance.GetItemCount(eggItemName);

        Debug.Log($"You cut a {eggItemName}. You now have {currentEgg}/{eggRequired}.");

        if (currentEgg >= eggRequired)
        {
            Debug.Log("You have collected enough eggs!");
            IsComplete = true;
            CompleteTask();
        }
    }

    public override void CompleteTask()
    {
        TaskManager.completedTasks.Add(this);
        WorldProgression.Instance.ApplyReward("EggCollectingFinished");
        PlayerInventory.Instance.RemoveItem("Basket");
        Debug.Log("Eggcollecting task completed!");
    }

}

using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Woodcutting Task")]
public class WoodcuttingTask : TaskBase
{
    [SerializeField] private int woodRequired = 1;    // How much wood to collect
    [SerializeField] private string woodItemName = "Wood"; // Item name to give player
    [SerializeField] private string toolRequired = "Axe";  // Required item to perform task

    private bool taskDone = false;

    public override void StartTask()
    {
        Debug.Log($"Go to {TargetLocationName} with an {toolRequired}.");
        taskDone = false;
    }

    public override void PerformTask()
    {
        if (taskDone) return;
        string heldItem = PlayerInventory.Instance.GetHeldItem();

        if (heldItem != toolRequired)
        {
            Debug.Log($"You need a {toolRequired} to cut wood!");
            return;
        }

        PlayerInventory.Instance.AddItem(woodItemName, 1);
        int currentWood = PlayerInventory.Instance.GetItemCount(woodItemName);

        Debug.Log($"You cut a {woodItemName}. You now have {currentWood}/{woodRequired}.");

        if (currentWood >= woodRequired)
        {
            Debug.Log("You have collected enough wood!");
            taskDone = true;
            CompleteTask();
        }
    }

    public override void CompleteTask()
    {
        if (!taskDone)
        {
            Debug.Log("You haven't completed the woodcutting yet.");
            return;
        }

        Debug.Log("Woodcutting task completed!");
    }

}

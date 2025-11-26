using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/LetterFinding")]
public class LetterFinding : TaskBase
{
    [SerializeField] private int lettersRequired = 1;    // How much wood to collect
    [SerializeField] private string letterItemName = "Letter"; // Item name to give player
    [SerializeField] private string toolRequired = "Piece of a letter";  // Required item to perform task

    public override void StartTask()
    {
        Debug.Log($"Go to {TargetLocationName} with an {toolRequired}.");
        PlayerInventory.Instance.AddItem("Piece of a letter");
        IsComplete = false;
    }

    public override void PerformTask()
    {
        if (IsComplete) return;
        string heldItem = PlayerInventory.Instance.GetHeldItem();

        if (!PlayerInventory.Instance.HasItem(toolRequired))
        {
            Debug.Log($"You need a {toolRequired} to cut wood!");
            return;
        }

        PlayerInventory.Instance.AddItem(letterItemName, 1);
        int currentLetters = PlayerInventory.Instance.GetItemCount(letterItemName);

        Debug.Log($"You cut a {letterItemName}. You now have {currentLetters}/{lettersRequired}.");

        if (currentLetters >= lettersRequired)
        {
            Debug.Log("You have collected all the letters! Go talk to Tanti Iana!");
            IsComplete = true;
            CompleteTask();
        }
    }

    public override void CompleteTask()
    {
        TaskManager.completedTasks.Add(this);
        WorldProgression.Instance.ApplyReward("LetterFindingFinished");
        PlayerInventory.Instance.RemoveItem("Piece of a letter");
        Debug.Log("LetterFinding task completed!");
    }

}

using HeneGames.DialogueSystem;
using UnityEngine;

public class TantiGetaReward : MonoBehaviour, IInteractable
{
    [Header("Tanti Geta Reward Settings")]
    public string rewardCode; // Set this in the inspector for each version
    public int interactionCountRequired = 1; // Optional: how many times you need to talk before reward

    private int currentInteractionCount = 0;

    public void Interact()
    {
        currentInteractionCount++;

        // Only give reward after enough interactions
        if (currentInteractionCount >= interactionCountRequired)
        {
            // Apply the reward via WorldProgression
            if (WorldProgression.Instance != null)
            {
                WorldProgression.Instance.ApplyReward(rewardCode);
            }

            // Optionally reset counter or disable this script
            // this.enabled = false;
        }
    }
}
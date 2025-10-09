using UnityEngine;

public class CelebrationCutSceneTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure it only triggers once and only for the player
        if (hasTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered the celebration cutscene trigger.");

            // Trigger the celebration / final cutscene
            if (WorldProgression.Instance != null)
            {
                WorldProgression.Instance.TriggerFinalCutscene();
            }
            else
            {
                Debug.LogWarning("WorldProgression instance not found — cannot trigger cutscene.");
            }

            // Optional: disable this object after triggering so it doesn’t replay
            gameObject.SetActive(false);
        }
    }
}

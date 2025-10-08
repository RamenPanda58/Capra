using UnityEngine;

public class TantiDidinaLocationTrigger : MonoBehaviour
{
    private bool hasTriggered = false;


    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered TantiDidinaLocation!");

            // Tell WorldProgression to play the cutscene
            if (WorldProgression.Instance != null)
            {
                WorldProgression.Instance.TriggerTantiDidinaCutscene();
            }
            else
            {
                Debug.LogWarning("WorldProgression instance not found!");
            }

            // Optionally disable the trigger so it doesn't fire again
            gameObject.SetActive(false);
        }
    }
}

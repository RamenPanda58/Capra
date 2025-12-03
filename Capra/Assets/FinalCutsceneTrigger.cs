using UnityEngine;

public class FinalCutsceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the FINAL CUTSCENE trigger!");
            WorldProgression.Instance.TriggerFinalCutscene();
        }
    }
}

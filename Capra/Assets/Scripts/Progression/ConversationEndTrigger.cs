using UnityEngine;
using UnityEngine.Events;

public class ConversationEndTrigger : MonoBehaviour
{
    [Header("What should be activated after conversation?")]
    public GameObject objectToActivate;

    [Header("Extra actions to call when the conversation ends")]
    public UnityEvent onConversationEnd;

    // Call this from your dialogue system when the conversation finishes
    public void TriggerConversationEnd()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Invoke any inspector-assigned events
        onConversationEnd?.Invoke();
    }
}

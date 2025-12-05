using UnityEngine;

public class PlayAudioOnCollision : MonoBehaviour
{
    [Header("Assign the object to activate on trigger")]
    public GameObject objectToActivate;

    [Header("Tag of the player object")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            else
            {
                Debug.LogWarning("PlayAudioOnCollision: No object assigned to activate.");
            }
        }
    }
}

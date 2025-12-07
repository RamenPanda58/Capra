using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentTarget;
    private IReadable currentReadable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            currentTarget.Interact();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentReadable != null)
        {
            currentReadable.Read();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && currentReadable != null)
        {
            currentReadable.Close();
            currentReadable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            currentTarget = interactable;

            if (other.TryGetComponent(out IReadable readable))
                currentReadable = readable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable) && interactable == currentTarget)
            currentTarget = null;
    }
}

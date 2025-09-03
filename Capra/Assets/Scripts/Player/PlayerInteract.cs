using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable currentTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable) && interactable == currentTarget)
        {
            currentTarget = null;
        }
    }
}

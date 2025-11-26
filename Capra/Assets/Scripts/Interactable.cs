using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    public string promptMessage = "Press E to collect, R to read and ESC to close";

    public abstract void Interact();
}

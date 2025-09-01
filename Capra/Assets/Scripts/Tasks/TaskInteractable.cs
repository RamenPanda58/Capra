using UnityEngine;

public class TaskInteractable : MonoBehaviour, IInteractable
{
    [Header("Optional Settings")]
    [Tooltip("Should this object be destroyed when interacted with?")]
    [SerializeField] private bool destroyOnInteract = false;

    private TaskManager taskManager;
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    public void Interact()
    {
        if (taskManager.TryPerformTask()) 
        {
            if (destroyOnInteract) 
            {
                Destroy(gameObject);
            }
        }
    }
}
using UnityEngine;
using TMPro;
using System.Collections;

public class TaskInteractable : Interactable
{
    [Header("Optional Settings")]
    [Tooltip("Should this object be destroyed when interacted with?")]
    [SerializeField] private bool destroyOnInteract = false;

    private TaskManager taskManager;

    [Header("Interaction Prompt Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    

    private void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractionPromptManager.Instance.Show(promptMessage, fadeDuration);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InteractionPromptManager.Instance.Hide(fadeDuration);
    }

    public override void Interact()
    {
        if (taskManager.TryPerformTask())
        {
            if (destroyOnInteract)
            {
                // ask manager to hide, then destroy this interactable
                InteractionPromptManager.Instance.Hide(fadeDuration);
                Destroy(gameObject);
                return;
            }

            InteractionPromptManager.Instance.Hide(fadeDuration);
        }
    }



}

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
    [SerializeField] private TextMeshProUGUI promptText;   // Reference to shared UI text
    [SerializeField] private string promptMessage = "Press E";
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine fadeRoutine;
    private bool playerInRange = false;

    private void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();

        if (promptText != null)
            SetPromptAlpha(0f); // start invisible
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (taskManager.TryPerformTask())
        {
            if (destroyOnInteract)
                Destroy(gameObject);

            HidePrompt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // make sure your player has this tag
        {
            playerInRange = true;
            ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (promptText == null) return;

        promptText.text = promptMessage;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadePrompt(1f, fadeDuration)); // fade in
    }

    private void HidePrompt()
    {
        if (promptText == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadePrompt(0f, fadeDuration)); // fade out
    }

    private IEnumerator FadePrompt(float targetAlpha, float duration)
    {
        Color startColor = promptText.color;
        float startAlpha = startColor.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            Color newColor = promptText.color;
            newColor.a = newAlpha;
            promptText.color = newColor;
            yield return null;
        }

        Color finalColor = promptText.color;
        finalColor.a = targetAlpha;
        promptText.color = finalColor;
    }

    private void SetPromptAlpha(float a)
    {
        Color c = promptText.color;
        c.a = a;
        promptText.color = c;
    }


}

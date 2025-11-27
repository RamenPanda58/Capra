using TMPro;
using UnityEngine;

public class Letter : Interactable, IReadable
{
    [Header("Letter Content")]
    [TextArea]
    public string letterText;

    public GameObject letterUIPanel;
    public TextMeshProUGUI uiText;

    [Header("Interaction Prompt Settings")]
    [SerializeField] private float fadeDuration = 0.3f;

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
        LetterManager.Instance.OnLetterCollected();

        InteractionPromptManager.Instance.Hide(fadeDuration);

        // Close UI if open
        Close();

        // Destroy this letter object in the world
        Destroy(gameObject);
    }

    public void Read()
    {
        // When pressing R
        letterUIPanel.SetActive(true);
        uiText.enabled = true;
        uiText.text = letterText;
    }

    public void Close()
    {
        // When pressing Escape
        if (letterUIPanel != null)
            letterUIPanel.SetActive(false);

        uiText.enabled = false;
    }
}

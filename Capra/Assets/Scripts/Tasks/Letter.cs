using TMPro;
using UnityEngine;

public class Letter : Interactable, IReadable
{
    [Header("Letter Content")]
    public GameObject letterUIPanel;

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

        // Add to PlayerInventory (your current system)
        PlayerInventory.Instance.AddItem(this.name, 1);

        // Add to hotbar (new system)
        HotbarInventory.Instance.AddLetter(new LetterData { text = letterUIPanel });

        Close();
        Destroy(gameObject);
    }


    public void Read()
    {
        // When pressing R
        letterUIPanel.SetActive(true);
    }

    public void Close()
    {
        // When pressing Escape
        if (letterUIPanel != null)
            letterUIPanel.SetActive(false);
    }
}

public class LetterData
{
    public GameObject text;
}

using TMPro;
using UnityEngine;

public class Letter : Interactable, IReadable
{
    [TextArea]
    public string letterText;

    public GameObject letterUIPanel;   // Assign your UI Panel prefab or object
    public TextMeshProUGUI uiText;

    public override void Interact()
    {
        // Notify manager
        LetterManager.Instance.OnLetterCollected();

        Close();
        Destroy(gameObject);
    }

    public void Read()
    {
        letterUIPanel.SetActive(true);
        uiText.enabled = true;
        uiText.text = letterText;
    }

    public void Close()
    {
        if (letterUIPanel != null)
            letterUIPanel.SetActive(false);
        uiText.enabled = false; 
    }
}

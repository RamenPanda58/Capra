using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [Header("References")]
    private TMP_Text textBox;

    [Header("Settings")]
    public float charactersPerSecond = 30f;
    public float punctuationDelay = 0.3f;
    public bool allowSkip = true;
    public KeyCode skipKey = KeyCode.Mouse1;

    private Coroutine typeCoroutine;
    private bool isTyping = false;
    private bool isSkipping = false;

    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        StartTyping();
    }

    public void StartTyping()
    {
        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);

        typeCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        isSkipping = false;

        textBox.maxVisibleCharacters = 0;
        int total = textBox.textInfo.characterCount;

        // Force TMP to update so we get the right character count
        textBox.ForceMeshUpdate();
        total = textBox.textInfo.characterCount;

        for (int i = 0; i < total; i++)
        {
            if (isSkipping)
            {
                textBox.maxVisibleCharacters = total;
                break;
            }

            textBox.maxVisibleCharacters = i + 1;

            char c = textBox.text[i];

            if (c == '.' || c == ',' || c == '!' || c == '?' || c == ':' || c == ';')
                yield return new WaitForSeconds(punctuationDelay);
            else
                yield return new WaitForSeconds(1f / charactersPerSecond);
        }

        isTyping = false;
    }

    void Update()
    {
        if (allowSkip && isTyping && Input.GetKeyDown(skipKey))
        {
            isSkipping = true;
        }
    }
}

using UnityEngine;
using TMPro;
using System.Collections;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI promptText;
    private Coroutine fadeRoutine;
    private float defaultFade = 0.3f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Show(string message, float fadeDuration = -1f)
    {
        if (promptText == null) return;
        promptText.text = message;
        StartFade(1f, fadeDuration < 0 ? defaultFade : fadeDuration);
    }

    public void Hide(float fadeDuration = -1f)
    {
        if (promptText == null) return;
        StartFade(0f, fadeDuration < 0 ? defaultFade : fadeDuration);
    }

    private void StartFade(float target, float duration)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadePrompt(target, duration));
    }

    private IEnumerator FadePrompt(float targetAlpha, float duration)
    {
        Color start = promptText.color;
        float startA = start.a;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, targetAlpha, t / duration);
            Color c = promptText.color;
            c.a = a;
            promptText.color = c;
            yield return null;
        }
        Color final = promptText.color;
        final.a = targetAlpha;
        promptText.color = final;
    }

    // optional instant hide
    public void SetAlpha(float a)
    {
        if (promptText == null) return;
        Color c = promptText.color;
        c.a = a;
        promptText.color = c;
    }
}

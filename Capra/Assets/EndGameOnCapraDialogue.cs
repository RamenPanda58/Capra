using UnityEngine;
using TMPro;
using System.Collections;

public class EndGameOnCapraDialogue : MonoBehaviour
{
    [Header("References")]
    public GameObject blackScreenUI;   // The UI panel with CanvasGroup
    public GameObject endGameTextObj;  // The text object to enable
    public float fadeDuration = 2f;    // How long the fade takes

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (blackScreenUI != null)
        {
            canvasGroup = blackScreenUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;

            blackScreenUI.SetActive(false);
        }

        if (endGameTextObj != null)
            endGameTextObj.SetActive(false);
    }

    // Call this function from the Inspector-friendly UnityEvent
    public void TriggerEndScene()
    {
        if (blackScreenUI != null)
            blackScreenUI.SetActive(true);

        if (endGameTextObj != null)
            endGameTextObj.SetActive(true);

        if (canvasGroup != null)
            StartCoroutine(FadeCanvasGroupIn());
    }

    private IEnumerator FadeCanvasGroupIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

            elapsed += Time.unscaledDeltaTime; // use unscaled time in case you pause time later
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        // Optional: Freeze time
        Time.timeScale = 0f;
    }
}

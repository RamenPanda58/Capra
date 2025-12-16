using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public float endY = 1200f; // where credits stop

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y >= endY)
        {
            // Optional: load menu or quit
            // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}

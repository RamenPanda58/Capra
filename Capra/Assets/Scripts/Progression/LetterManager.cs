using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterManager : MonoBehaviour
{
    public static LetterManager Instance;

    public int lettersCollected = 0;
    public int totalLetters = 7;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnLetterCollected()
    {
        lettersCollected++;

        Debug.Log("Collected letters: " + lettersCollected);

        WorldProgression.Instance.ApplyProgression(lettersCollected);

        if (lettersCollected >= totalLetters)
        {
            LoadEndingScene();
        }
    }

    private void LoadEndingScene()
    {
        SceneManager.LoadScene("EndingScene");
    }
}

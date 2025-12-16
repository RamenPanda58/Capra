using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterManager : MonoBehaviour
{
    public static LetterManager Instance;

    public int lettersCollected = 0;
    public int totalLetters = 7;

    [Header("Objects unlocked per letter")]

    public GameObject letter1Object;
    public GameObject letter2Object;
    public GameObject letter3Object;
    public GameObject letter4Object;
    public GameObject letter5Object;
    public GameObject letter6Object;
    public GameObject letter7Object;

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

        ActivateLetterObject(lettersCollected);

        WorldProgression.Instance.ApplyProgression(lettersCollected);

        if (lettersCollected >= totalLetters)
            LoadEndingScene();
    }

    private void ActivateLetterObject(int letterNumber)
    {
        switch (letterNumber)
        {
            case 1:
                if (letter1Object) letter1Object.SetActive(true);
                break;
            case 2:
                if (letter2Object) letter2Object.SetActive(true);
                break;
            case 3:
                if (letter3Object) letter3Object.SetActive(true);
                break;
            case 4:
                if (letter4Object) letter4Object.SetActive(true);
                break;
            case 5:
                if (letter5Object) letter5Object.SetActive(true);
                break;
            case 6:
                if (letter6Object) letter6Object.SetActive(true);
                break;
            case 7:
                if (letter7Object) letter7Object.SetActive(true);
                break;
        }
    }

    private void LoadEndingScene()
    {
        SceneManager.LoadScene("EndingScene");
    }
}

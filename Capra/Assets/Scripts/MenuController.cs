using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("3.0Prototype 1");
    }

    public void QuitGame()
    {
        // Quit in built game
        Application.Quit();

        // Quit in editor (won't be included in build)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

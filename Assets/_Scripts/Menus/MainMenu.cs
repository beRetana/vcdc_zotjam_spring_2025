using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public delegate void GameStarting();
    public static GameStarting OnGameStarting;

    public void StartGame()
    {
        OnGameStarting?.Invoke();
        FindFirstObjectByType<SceneTransition>().NextScene("Forest_S1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

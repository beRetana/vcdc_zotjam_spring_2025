using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private const string _sceneName = "Forest_S1";

    public void StartGame()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

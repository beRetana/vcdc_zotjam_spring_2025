using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

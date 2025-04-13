using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Forest_F1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

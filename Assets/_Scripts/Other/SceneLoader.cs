using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public delegate void FinishedBattle(int nextScene);
    public static FinishedBattle OnFinishedBattle;

    public static int nextLoadSceneInt = 0;

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    public void LoadLastScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex - 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigged collision");
        OnFinishedBattle?.Invoke(++nextLoadSceneInt);
        //LoadNextScene();
    }
}

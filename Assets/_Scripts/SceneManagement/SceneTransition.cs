using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public enum SceneTypeEnum { Mid, Game };
    public SceneTypeEnum sceneTypeEnum { get; private set; }

    public delegate void SceneLoaded();
    public static SceneLoaded OnSceneLoaded;

    public string nextSceneName = "Forest_S1";

    public delegate void StartGamePlay();
    public static StartGamePlay OnStartGamePlay;

    void Start()
    {
        canvasGroup.alpha = 1f;
        //SetUpEnterMidScene();
        //LoadNextSceneInBackground();
        //OnStartGamePlay?.Invoke();
    }


    public void NextScene(string nextSceneStringName)
    {
        nextSceneName = nextSceneStringName;
        if(nextSceneName == "Forest_S1")
        {
            canvasGroup.alpha = 1f;
            SceneManager.LoadScene("Forest_S1");
            OnStartGamePlay?.Invoke();
            Debug.Log("SATAGEFINO)");
            return;
        }
        StartCoroutine(ExitCurrentSceneLoadNextScene());
    }

    IEnumerator ExitCurrentSceneLoadNextScene()
    {
        SetUpEnterMidScene();
        yield return new WaitForSeconds(2.5f);
        LoadNextSceneInBackground();
        // should still be greyed out
    }

    public void EnterGameScene()
    {
        //open the scene
        SetUpEnterGameScene();
    }



    private void SetUpEnterMidScene()
    {
        StartCoroutine(AlphaToOpaque());
    }

    private void SetUpEnterGameScene()
    {
        StartCoroutine(AlphaToTransparent());
    }

    

    private void LoadNextSceneInBackground()
    {
        SceneManager.LoadScene(sceneName: nextSceneName);
    }


    IEnumerator AlphaToTransparent()
    {
        canvasGroup.alpha = 1f;
        float transitionTime = 2f;

        while (transitionTime >= 0f)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, 1 - transitionTime / 2);

            transitionTime -= Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        OnSceneLoaded?.Invoke();
    }

    IEnumerator AlphaToOpaque()
    {
        canvasGroup.alpha = 0f;

        float transitionTime = 2f;

        while (transitionTime >= 0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, 1 - transitionTime / 2);

            transitionTime -= Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}

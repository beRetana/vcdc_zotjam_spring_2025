using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] DialogueParser dialogueParser;
    [SerializeField] SceneTransition sceneTransition;

    public delegate void StartSpawning();
    public static StartSpawning OnStartSpawning;

    private string currentEvent;


    public void OnEnable()
    {
        MessageQueue.OnEventMessageCompletion += FinishDialogue;


        SceneTransition.OnStartGamePlay += FirstDialogue;
        SceneLoader.OnFinishedBattle += OpenLoadingScreen;
        // On Win_conner += LoadingScreen;
        // On Win_city += LoadingScreen2;
    }


    private void FirstDialogue()
    {
        OpenDialogue(0, "Opening");
        Debug.Log("OPENING DIALOGUE");
    }

    private void OpenLoadingScreen(int sceneToLoad)
    {
        if (sceneToLoad == 1) Debug.LogError("loading nothing");//;//LoadingScreen();
        else if (sceneToLoad == 2) LoadingScreen();
        else if (sceneToLoad == 3) LoadingScreen2();
            //sceneTransition.NextScene("Church_3");
    }

    private void LoadingScreen()
    {
        Debug.LogWarning("LOADING SCREEN 1");
        OpenDialogue(1, "LoadingScreen");
        sceneTransition.NextScene("City_2");
    }

    private void LoadingScreen2()
    {
        Debug.LogWarning("LOADING SCREEN 2");

        OpenDialogue(3, "LoadingScreen2");
        sceneTransition.NextScene("Church_3");
    }



    public void FinishDialogue()
    {
        switch(currentEvent)
        {
            case "Opening":
                sceneTransition.EnterGameScene();
                OpenDialogue(0, "ExitHouse");
                OnStartSpawning?.Invoke();
                // TK Spawn enemies
                break;

            case "ExitHouse":
                //this is after the dialogue ends, but we must continue fighting

                //OpenDialogue(1, "LoadingScreen"); //TK DELETE
                //sceneTransition.NextScene("City_2"); //TK DELETE

                break;

            case "LoadingScreen":
                sceneTransition.EnterGameScene();
                OpenDialogue(2, "CityLevel");
                OnStartSpawning?.Invoke();
                break;

            case "CityLevel":
                //this is after the dialogue ends, but we must continue fighting

                //OpenDialogue(3, "LoadingScreen2"); //TK DELETE
                //sceneTransition.NextScene("Church_3"); //TK DELETE


                break;

            case "LoadingScreen2":
                sceneTransition.EnterGameScene();
                OpenDialogue(4, "WeddingLevel");
                OnStartSpawning?.Invoke();
                break;

            case "WeddingLevel":
                //this is after the dialogue ends, but we must continue fighting
                break;

        }
    }

    private void OpenDialogue(int sceneIndex, string eventName)
    {
        Debug.Log("ope");
        currentEvent = eventName;
        dialogueParser.OpenDialogue(sceneIndex, eventName);
    }
}
/*
0   ExitHouse
1   LoadingScreen
2   CityLevel
3   LoadingScreen2
4   WeddingLevel
*/
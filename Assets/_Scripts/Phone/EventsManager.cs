using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] DialogueParser dialogueParser;
    [SerializeField] SceneTransition sceneTransition;

    private string currentEvent;


    public void OnEnable()
    {
        MessageQueue.OnEventMessageCompletion += FinishDialogue;


        SceneTransition.OnStartGamePlay += FirstDialogue;
        // On Win_conner += LoadingScreen;
        // On Win_city += LoadingScreen2;
    }

    private void FirstDialogue()
    {
        OpenDialogue(0, "Opening");
    }

    private void LoadingScreen()
    {
        OpenDialogue(1, "LoadingScreen");
        sceneTransition.NextScene("city");
    }

    private void LoadingScreen2()
    {
        OpenDialogue(3, "LoadingScreen2");
        sceneTransition.NextScene("church");
    }



    public void FinishDialogue()
    {
        switch(currentEvent)
        {
            case "Opening":
                sceneTransition.EnterGameScene();
                OpenDialogue(0, "ExitHouse");
                // TK Spawn enemies
                break;

            case "ExitHouse":
                //this is after the dialogue ends, but we must continue fighting

                OpenDialogue(1, "LoadingScreen"); //TK DELETE
                sceneTransition.NextScene("city"); //TK DELETE

                break;

            case "LoadingScreen":
                sceneTransition.EnterGameScene();
                OpenDialogue(2, "CityLevel");
                break;

            case "CityLevel":
                //this is after the dialogue ends, but we must continue fighting

                OpenDialogue(3, "LoadingScreen2"); //TK DELETE
                sceneTransition.NextScene("church"); //TK DELETE


                break;

            case "LoadingScreen2":
                sceneTransition.EnterGameScene();
                OpenDialogue(4, "WeddingLevel");
                break;

            case "WeddingLevel":
                //this is after the dialogue ends, but we must continue fighting
                break;

        }
    }

    private void OpenDialogue(int sceneIndex, string eventName)
    {
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
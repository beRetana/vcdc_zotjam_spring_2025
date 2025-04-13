using UnityEngine;

public class EventsManager : MonoBehaviour
{
    [SerializeField] DialogueParser dialogueParser;

    public void OnEnable()
    {
        CSVReader.OnFinishedReadingDialogue += FirstDialogue;
    }

    private void FirstDialogue()
    {
        dialogueParser.OpenDialogue(0, "Opening");
    }

    public void StartDialogueEvent(string eventName)
    {
        switch(eventName)
        {
            case "ExitHouse":

                break;

            case "LoadingScreen":
                break;

            case "CityLevel":

                break;

            case "LoadingScreen2":
                break;

            case "WeddingLevel":
                break;

        }
    }
}
/*
0   ExitHouse
1   LoadingScreen
2   CityLevel
3   LoadingScreen2
4   WeddingLevel
*/
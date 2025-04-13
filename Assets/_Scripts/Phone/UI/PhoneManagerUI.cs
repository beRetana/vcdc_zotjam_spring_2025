using System.Collections.Generic;
using UnityEngine;

public class PhoneManagerUI : MonoBehaviour
{
    [SerializeField] private DialogueOptionsManager _optionsManager;
    [SerializeField] private MessagesManager _messagesManager;
    [SerializeField] private FaceUI _faceManager;
    

    private List<CSVReader.DialogueRow> tests;
    private int indexer;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }
    /*
    private void Start()
    {   
        tests = new List<CSVReader.DialogueRow>();
        CSVReader.DialogueRow test = new CSVReader.DialogueRow();
        test.character = CSVReader.CharacterEnum.Her;
        test.dialogue = "This is only for testing lol"; // 24, one 2
        CSVReader.DialogueRow test1 = new CSVReader.DialogueRow();
        test1.character = CSVReader.CharacterEnum.Me;
        test1.dialogue = "This is only for testing but this should be a very very and really long message"; // 79, one 3 and one 1
        CSVReader.DialogueRow test2 = new CSVReader.DialogueRow();
        test2.character = CSVReader.CharacterEnum.Her;
        test2.dialogue = "This is only for testing this should be medium"; // 46, one 2
        CSVReader.DialogueRow test3 = new CSVReader.DialogueRow();
        test3.character = CSVReader.CharacterEnum.Her;
        test3.dialogue = "This is only for testing but intersting"; // 39, one 2
        CSVReader.DialogueRow test4 = new CSVReader.DialogueRow();
        test4.character = CSVReader.CharacterEnum.Me;
        test4.dialogue = "lol"; // 3, one 1
        CSVReader.DialogueRow test5 = new CSVReader.DialogueRow();
        test5.character = CSVReader.CharacterEnum.Her;
        test5.dialogue = "This is only for testing this should be medium"; // 46, one 2
        CSVReader.DialogueRow test6 = new CSVReader.DialogueRow();
        test6.character = CSVReader.CharacterEnum.Her;
        test6.dialogue = "This is only for testing but intersting"; // 39, one 2
        CSVReader.DialogueRow test7 = new CSVReader.DialogueRow();
        test7.character = CSVReader.CharacterEnum.Me;
        test7.dialogue = "lol"; // 3, one 1

        tests.Add(test);
        tests.Add(test1);
        tests.Add(test2);
        tests.Add(test3);
        tests.Add(test4);
        tests.Add(test5);
        tests.Add(test6);
        tests.Add(test7);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DisplayMessage(tests[indexer]);
            ++indexer;
        }
    }  */

    public void DisplayOptionsUI(CSVReader.DialogueRow[] dialogueOptions)
    {
        _optionsManager.DisplayResponseOptions(dialogueOptions);
    }

    public void DisplayStringOptionsUI(string[] dialogueStringOptions)
    {
        _optionsManager.DisplayResponseStringOptions(dialogueStringOptions);
    }

    public void DisplayNoOptionsUI()
    {
        _optionsManager.DisableResponseOptions();
    }

    public void DisplayMessage(CSVReader.DialogueRow dialogueRow)
    {
        if(dialogueRow.character == CSVReader.CharacterEnum.Her)
        {
            //send
            AudioManager.instance.PlayOneShot(FMODEvents.instance.MessageReceived,this.transform.position);
        }
        else if(dialogueRow.character == CSVReader.CharacterEnum.Me)
        {
            //receive
            AudioManager.instance.PlayOneShot(FMODEvents.instance.MessageReceived,this.transform.position);
        }
        _messagesManager.SendMessage(dialogueRow);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class DialogueParser : MonoBehaviour
{
    [SerializeField] CSVReader _CSVReader;

    private LinkedList<CSVReader.DialogueRow> _dialogueList;
    private string[] responses;

    private CSVReader.TypeEnum responseType;
    public bool questionOpen { get; private set; }
    public bool responseOpen { get; private set; }


    SceneResponses sceneResponses;

    public struct SceneResponses
    {
        public Dictionary<int, string[]> sceneToResponses;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CSVReader.OnFinishedReadingDialogue += OpenDialogue;
    }

    private void OpenDialogue()
    {
        BeginDialogue(0, "Opening");
    }

    private void BeginDialogue(int sceneNum, string eventName)
    {
        IEnumerable<CSVReader.DialogueRow> dialogueRowIE = _CSVReader.GetEventDialogueList(sceneNum, eventName);

        PopulateDialogueList(dialogueRowIE);


        PlayDialogue();
    }

    private bool PopulateDialogueList(IEnumerable<CSVReader.DialogueRow> dialogueRowIE)
    {
        _dialogueList = new LinkedList<CSVReader.DialogueRow>();
        responseType = CSVReader.TypeEnum.Std;

        sceneResponses = new SceneResponses()
        {
            sceneToResponses = new Dictionary<int, string[]>()
        };

        int index = 0;
        int q_index = -1;
        responses = new string[3];

        CSVReader.TypeEnum prevType = CSVReader.TypeEnum.Std;

        foreach(CSVReader.DialogueRow r in dialogueRowIE)
        {
            if (r.type != prevType)
            {
                prevType = r.type;

                if (r.type == CSVReader.TypeEnum.R1)
                {
                    sceneResponses.sceneToResponses.Add(r.sectionIndex, new string[3]);
                    sceneResponses.sceneToResponses[r.sectionIndex][0] = r.dialogue;
                }
                else if (r.type == CSVReader.TypeEnum.R2)
                {
                    sceneResponses.sceneToResponses[r.sectionIndex][1] = r.dialogue;
                }
                else if (r.type == CSVReader.TypeEnum.R3)
                {
                    sceneResponses.sceneToResponses[r.sectionIndex][2] = r.dialogue;
                    Debug.Log($"{r.sectionIndex}: [1] {sceneResponses.sceneToResponses[r.sectionIndex][0]} [2] {sceneResponses.sceneToResponses[r.sectionIndex][1]} [3] {sceneResponses.sceneToResponses[r.sectionIndex][2]}");
                }

            }
            _dialogueList.AddLast(r);

            ++index;
        }

        return q_index >= 0;
    }

    private void NextDialogue()
    {
        if (RemoveCurrentDialogue())
            PlayDialogue();

    }

    private bool RemoveCurrentDialogue()
    {
        //Debug.Log($"Deleting:  {_dialogueList.Count} -> {_dialogueList.Count -1}");
        if (_dialogueList.Count <= 1)
        {
            EndDialogue();
            return false;
        }
        _dialogueList.RemoveFirst();
        return true;
    }


    private void EndDialogue()
    {
        Debug.Log("END DIALOGUE");
    }

    private void PlayDialogue()
    {
        EvaluateDialogueRow(_dialogueList.First.Value);
    }

    private void EvaluateDialogueRow(CSVReader.DialogueRow r)
    {
        //Debug.Log("TYPE" + r.type);
        switch (r.type)
        {
            case CSVReader.TypeEnum.Std:
                Debug.Log(r);
                NextDialogue();
                break;

            case CSVReader.TypeEnum.Q:
                //Debug.Log("QUESTION");
                Debug.Log(r);
                OpenQuestion(r);
                //NextDialogue();
                break;


                
            case CSVReader.TypeEnum.R1:
                Debug.Log(r);
                break;
            case CSVReader.TypeEnum.R2:
                Debug.Log(r);
                break;
            case CSVReader.TypeEnum.R3:
                Debug.Log(r);
                break;


            case CSVReader.TypeEnum.Wait:
                Debug.Log($"Wait for {r.dialogue} seconds");
                NextDialogue();
                break;
        }
    }

    private void OpenQuestion(CSVReader.DialogueRow r)
    {
        questionOpen = true;
        Debug.Log($"[1] {sceneResponses.sceneToResponses[r.sectionIndex][0]}");
        Debug.Log($"[2] {sceneResponses.sceneToResponses[r.sectionIndex][1]}");
        Debug.Log($"[3] {sceneResponses.sceneToResponses[r.sectionIndex][2]}");
    }

    //private void PlayDialogueLine()

    public bool TakeAction(int actionNum) // 1 2 or 3
    {
        if (!questionOpen) return false;


        Debug.Log($"[{actionNum}] {responses[actionNum - 1]}");

        

        if (actionNum == 1) responseType = CSVReader.TypeEnum.R1;
        else if (actionNum == 2) responseType = CSVReader.TypeEnum.R2;
        else if (actionNum == 3) responseType = CSVReader.TypeEnum.R3;

        GoToResponse(responseType);

        questionOpen = false;
        return true;
    }

    private void GoToResponse(CSVReader.TypeEnum type)
    {
        responseType = CSVReader.TypeEnum.Std;
        //PlayDialogue();
        while (_dialogueList.First != null && _dialogueList.First.Value.type != type)
        {
            RemoveCurrentDialogue();

            if(_dialogueList.First == null)
            {
                EndDialogue();
                return;
            }
        }
        PlayAllOfResponse(type);

    }

    private void PlayAllOfResponse(CSVReader.TypeEnum type)
    {
        while (_dialogueList.First != null && _dialogueList.First.Value.type == type)
        {
            //EvaluateDialogueRow(_dialogueList.First.Value);
            PlayDialogue();
            RemoveCurrentDialogue();
        }

        while (_dialogueList.First != null && _dialogueList.First.Value.type == CSVReader.TypeEnum.R2)
        {
            RemoveCurrentDialogue();
        }
        while (_dialogueList.First != null && _dialogueList.First.Value.type == CSVReader.TypeEnum.R3)
        {
            RemoveCurrentDialogue();
        }


        PlayDialogue();
    }
}

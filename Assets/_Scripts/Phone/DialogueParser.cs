using UnityEngine;
using System.Collections.Generic;

public class DialogueParser : MonoBehaviour
{
    [SerializeField] CSVReader _CSVReader;

    private LinkedList<CSVReader.DialogueRow> _dialogueList;
    private CSVReader.TypeEnum responseType;

    public bool questionOpen { get; private set; }

    private Dictionary<int, string[]> sceneResponses;


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
        CSVReader.SectionsList eventSectionList = _CSVReader.GetEventDialogueListObject(sceneNum, eventName);

        _dialogueList = new LinkedList<CSVReader.DialogueRow>();
        sceneResponses = new Dictionary<int, string[]>();
        responseType = CSVReader.TypeEnum.Std;

        foreach (CSVReader.DialogueRowList section in eventSectionList.sectionsList)
        {
            foreach(CSVReader.DialogueRow row in section.dialogueRowList)
            {
                _dialogueList.AddLast(row);

                if( row.messageIndex == 0 && (row.type == CSVReader.TypeEnum.R1 || row.type == CSVReader.TypeEnum.R2 || row.type == CSVReader.TypeEnum.R3))
                {
                    if (!sceneResponses.ContainsKey(row.sectionIndex))
                        sceneResponses[row.sectionIndex] = new string[3];

                    int idx = (int)(row.type - CSVReader.TypeEnum.R1);
                    sceneResponses[row.sectionIndex][idx] = row.dialogue;
                }
            }
        }

        PlayDialogue();
    }

    private void PlayDialogue()
    {
        if(_dialogueList.Count == 0)
        {
            EndDialogue();
            return;
        }
        EvaluateDialogueRow(_dialogueList.First.Value);
    }

    private void EvaluateDialogueRow(CSVReader.DialogueRow r)
    {
        switch (r.type)
        {
            case CSVReader.TypeEnum.Std:
                SendMessage(r);
                RemoveCurrentDialogue();
                PlayDialogue();
                break;

            case CSVReader.TypeEnum.Wait:
                Debug.Log($"Wait for {r.dialogue} seconds");
                RemoveCurrentDialogue();
                PlayDialogue();
                break;


            case CSVReader.TypeEnum.Q:
                SendMessage(r);
                OpenQuestion(r.sectionIndex);
                break;

            case CSVReader.TypeEnum.R1:
                SendMessage(r);
                break;
            case CSVReader.TypeEnum.R2:
                SendMessage(r);
                break;
            case CSVReader.TypeEnum.R3:
                SendMessage(r);
                break;
        }
    }


    private void RemoveCurrentDialogue()
    {
        if (_dialogueList.Count > 0)
            _dialogueList.RemoveFirst();
    }


    private void EndDialogue()
    {
        Debug.Log("END DIALOGUE");
    }



    private void OpenQuestion(int sectionIndex)
    {
        questionOpen = true;

        if (sceneResponses.TryGetValue(sectionIndex, out string[] choices))
        {
            SendReponseOptions(choices);

            //Debug.Log($"[1] {choices[0]}");
            //Debug.Log($"[2] {choices[1]}");
            //Debug.Log($"[3] {choices[2]}");
        }
    }


    public bool TakeAction(int actionNum) // 1 2 or 3
    {
        //Debug.Log($"Attempt Take Action: {actionNum}. questionOpen = {questionOpen}");
        if (!questionOpen) return false;

        Debug.Log($"[{actionNum}]");

        CSVReader.TypeEnum selectedType = CSVReader.TypeEnum.R1 + (actionNum - 1);
        questionOpen = false;

        SkipToResponse(selectedType);
        return true;

    }

    private void SkipToResponse(CSVReader.TypeEnum type)
    {
        responseType = CSVReader.TypeEnum.Std;
        //PlayDialogue();
        while (_dialogueList.First != null && _dialogueList.First.Value.type != type)
        {
            RemoveCurrentDialogue();
        }

        while (_dialogueList.First != null && _dialogueList.First.Value.type == type)
        {
            SendMessage(_dialogueList.First.Value);
            RemoveCurrentDialogue();

        }

        while(_dialogueList.First != null &&
            (_dialogueList.First.Value.type == CSVReader.TypeEnum.R1 ||
            _dialogueList.First.Value.type == CSVReader.TypeEnum.R2 ||
            _dialogueList.First.Value.type == CSVReader.TypeEnum.R3))
        {
            RemoveCurrentDialogue();
        }

        PlayDialogue();
    }

    private void SendMessage(CSVReader.DialogueRow r)
    {
        Debug.Log(r);
    }

    private void SendReponseOptions(string[] reponseChoices)
    {
        Debug.Log($"[1] {reponseChoices[0]}");
        Debug.Log($"[2] {reponseChoices[1]}");
        Debug.Log($"[3] {reponseChoices[2]}");
    }
}

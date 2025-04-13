using System;
using UnityEngine;
using System.Collections.Generic;

public class CSVReader : MonoBehaviour
{
    const int TOTAL_COLS = 9;

    public delegate void FinishedReadingDialogue();
    public static event FinishedReadingDialogue OnFinishedReadingDialogue;

    public TextAsset textAssetData;

    public enum CharacterEnum { None, Her, Me };
    public enum TypeEnum { Std, Q, R1, R2, R3, Wait };

    public EventsDictionary myDialogueEventDictionary;



    [System.Serializable]
    public struct DialogueRow
    {
        public string rowName;
        public int scene;
        public string eventName;
        public int sectionIndex;
        public TypeEnum type;
        public int messageIndex;
        public CharacterEnum character;
        public string dialogue;
        public int lovePoints;

        public override String ToString()
        {
            return $"[{rowName}]:".PadRight(30) +
                $"{character}: {dialogue}".PadRight(40) +
                $"{((lovePoints == 0) ? $"" : ((lovePoints > 0) ? "+" + lovePoints : lovePoints))}";
        }

    }
    [System.Serializable]
    public struct DialogueRowList { public List<DialogueRow> dialogueRowList; }
    [System.Serializable]
    public struct SectionsList { public List<DialogueRowList> sectionsList; }
    [System.Serializable]
    public struct EventsDictionary { public Dictionary<string, SectionsList> eventsDictionary; }



    private void ReadCSV()
    {
        myDialogueEventDictionary = new EventsDictionary()
        {
            eventsDictionary = new Dictionary<string, SectionsList>()
        };

        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / TOTAL_COLS - 1;
        Debug.Log($"Table size = {tableSize}");

        string previousEvent = "";
        string currentEvent = "";




        SectionsList mySectionsList = new SectionsList()
        {
            sectionsList = new List<DialogueRowList>()
        };
        DialogueRowList currentDialogueRowList = new DialogueRowList()
        {
            dialogueRowList = new List<DialogueRow>()
        };

        int currentSectionIndex = -1;

        for (int i = 0; i < tableSize; ++i)
        {
            DialogueRow row = ReadRow(data, i);
            currentEvent = row.scene + "-" + row.eventName;


            if (!(String.Equals(currentEvent, previousEvent)) && (i != 0))
            {
                if (currentDialogueRowList.dialogueRowList.Count > 0)
                    mySectionsList.sectionsList.Add(currentDialogueRowList);

                //close event
                Debug.Log($"Adding EventDialogue list of size = {mySectionsList.sectionsList.Count} to key = {previousEvent}");
                myDialogueEventDictionary.eventsDictionary.Add(previousEvent, mySectionsList);

                mySectionsList = new SectionsList() { sectionsList = new List<DialogueRowList>() };
                currentDialogueRowList = new DialogueRowList() { dialogueRowList = new List<DialogueRow>() };
                currentSectionIndex = -1;
            }

            if(row.sectionIndex != currentSectionIndex)

            {
                if (currentDialogueRowList.dialogueRowList.Count > 0)
                    mySectionsList.sectionsList.Add(currentDialogueRowList);
                currentDialogueRowList = new DialogueRowList() { dialogueRowList = new List<DialogueRow>() };
                currentSectionIndex = row.sectionIndex;
            }

            currentDialogueRowList.dialogueRowList.Add(row);
            previousEvent = currentEvent;
            //mySectionsList.sectionsList.Add(ReadRow(data, i));
        }

        if (currentDialogueRowList.dialogueRowList.Count > 0)
            mySectionsList.sectionsList.Add(currentDialogueRowList);

        if (!string.IsNullOrEmpty(previousEvent))
            myDialogueEventDictionary.eventsDictionary.Add(previousEvent, mySectionsList);


        Debug.Log("Finished Reading Dialogue");

        //ReadSection();
        PrintAllDialogue();
        OnFinishedReadingDialogue?.Invoke();
    }



    //void ReadCSV()
    //{
    //    string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

    //    int tableSize = data.Length / TOTAL_COLS - 1;
    //    Debug.Log($"Table size = {tableSize}");

    //    myDialogueList.eventDialogueSections = new List<DialogueRow>();

    //    string previousEvent = "";
    //    string currentEvent = "";

    //    for (int i = 0; i < tableSize; ++i, previousEvent = currentEvent)
    //    {
    //        // check if NEW scene-eventName combo comes
    //        currentEvent = data[TOTAL_COLS * (i + 1) + 1] + "-" + data[TOTAL_COLS * (i + 1) + 2];

    //        if ( !(String.Equals(currentEvent, previousEvent)) && (i != 0))
    //        {
    //            Debug.Log($"Adding EventDialogue list of size = {myDialogueList.eventDialogueSections.Count} to key = {previousEvent}");
    //            myDialgogueEventDictionary.eventDictionary.Add(previousEvent, myDialogueList);

    //            myDialogueList = new EventDialogueSections()
    //            {
    //                eventDialogueSections = new List<DialogueRow>()
    //            };

    //        }

    //        myDialogueList.eventDialogueSections.Add(ReadRow(data, i));
    //    }
    //    Debug.Log("Finished Reading Dialogue");

    //    PrintAllDialogue();
    //    OnFinishedReadingDialogue?.Invoke();
    //}

    private void PrintAllDialogue()
    {
        Debug.Log("========== Dialogue ==========");

        foreach(KeyValuePair<string,SectionsList> entry in myDialogueEventDictionary.eventsDictionary)
        {
            Debug.Log($"\n=== Event: {entry.Key} ===");

            SectionsList eventSections = entry.Value;

            for (int i = 0; i < eventSections.sectionsList.Count; ++i)
            {
                DialogueRowList section = eventSections.sectionsList[i];

                Debug.Log($"--- Section{i} ---");

                foreach (DialogueRow row in section.dialogueRowList)
                {
                    Debug.Log($"    {row}");
                }
            }
        }
        Debug.Log("========== End of Dialogue ==========");
    }


    private DialogueRow ReadRow(string[] data, int i)
    {
        DialogueRow currentRow = new DialogueRow();

        currentRow.rowName = data[TOTAL_COLS * (i + 1)];

        currentRow.scene = TryParseInt(data[TOTAL_COLS * (i + 1) + 1]);
        currentRow.eventName = data[TOTAL_COLS * (i + 1) + 2];

        currentRow.sectionIndex = TryParseInt(data[TOTAL_COLS * (i + 1) + 3]);
        currentRow.type = (TypeEnum)Enum.Parse(typeof(TypeEnum), data[TOTAL_COLS * (i + 1) + 4]);
        currentRow.messageIndex = TryParseInt(data[TOTAL_COLS * (i + 1) + 5]);
        currentRow.character = (CharacterEnum)Enum.Parse(typeof(CharacterEnum), data[TOTAL_COLS * (i + 1) + 6]);
        currentRow.dialogue = data[TOTAL_COLS * (i + 1) + 7];
        currentRow.lovePoints = TryParseInt(data[TOTAL_COLS * (i + 1) + 8]);

        return currentRow;
    }

    public int TryParseInt(string value)
    {
        int result;
        return int.TryParse(value, out result) ? result : 0;
    }

    private void Start()
    {
        ReadCSV();
    }

    public SectionsList GetEventDialogueListObject(int sceneNum, string eventName)
    {
        string key = sceneNum + "-" + eventName;

        Debug.Log("KEYYYYY " + key);

        return myDialogueEventDictionary.eventsDictionary[key];
    }

    //public List<DialogueRowList> GetEventDialogueList(int sceneNum, string eventName)
    //{
    //    return myDialogueEventDictionary.eventsDictionary[sceneNum + "-" + eventName].sectionsList;
    //}

}

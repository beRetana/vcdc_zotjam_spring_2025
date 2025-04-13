using System;
using UnityEngine;
using System.Collections.Generic;

public class CSVReader : MonoBehaviour
{
    /// <summary>
    /// 
    /// EventDictionary
    ///     eventDictionary | dictionary<string, EventDialogue>
    ///
    /// EventDialogue
    ///     dialogueRows | List<DialogueRow>
    ///
    /// EventDictionary myEventDictionary
    /// myEventDictionary.eventDictionary[eventName].dialogueRows | every dialogue line with eventName = eventName                                 
    ///         
    /// </summary>
    const int TOTAL_COLS = 9;


    public delegate void FinishedReadingDialogue();
    public static event FinishedReadingDialogue OnFinishedReadingDialogue;

    public TextAsset textAssetData;

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
                $"{((lovePoints==0)? $"" : ((lovePoints>0)? "+" + lovePoints : lovePoints))}";
        }

    }

    public enum CharacterEnum { None, Her, Me };
    public enum TypeEnum { Std, Q, R1, R2, R3, Wait };


    [System.Serializable]
    public struct EventDialogue
    {
        public List<DialogueRow> dialogueRows;
    }

    // THIS IS THE BIG IMPORTANT PART LOOK HERE!!!!!!!
    [System.Serializable]
    public struct EventDictionary
    {
        public Dictionary<string, EventDialogue> eventDictionary;
    }




    public EventDictionary myDialgogueEventDictionary = new EventDictionary()
    {
        eventDictionary = new Dictionary<string, EventDialogue>()
    };

    public EventDialogue myDialogueList = new EventDialogue();

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / TOTAL_COLS - 1;
        Debug.Log($"Table size = {tableSize}");
        myDialogueList.dialogueRows = new List<DialogueRow>();

        string previousEvent = "";
        string currentEvent = "";

        for (int i = 0; i < tableSize; ++i, previousEvent = currentEvent)
        {
            // check if NEW scene-eventName combo comes
            currentEvent = data[TOTAL_COLS * (i + 1) + 1] + "-" + data[TOTAL_COLS * (i + 1) + 2];

            if ( !(String.Equals(currentEvent, previousEvent)) && (i != 0))
            {
                Debug.Log($"Adding EventDialogue list of size = {myDialogueList.dialogueRows.Count} to key = {previousEvent}");
                myDialgogueEventDictionary.eventDictionary.Add(previousEvent, myDialogueList);
                myDialogueList = new EventDialogue()
                {
                    dialogueRows = new List<DialogueRow>()
                };
                
            }

            myDialogueList.dialogueRows.Add(ReadRow(data, i));
        }

        Debug.Log("Finished Reading Dialogue");
        OnFinishedReadingDialogue?.Invoke();
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

    public EventDialogue GetEventDialogueListObject(int sceneNum, string eventName)
    {
        return myDialgogueEventDictionary.eventDictionary[sceneNum + "-" + eventName];
    }

    public IEnumerable<DialogueRow> GetEventDialogueList(int sceneNum, string eventName)
    {
        return myDialgogueEventDictionary.eventDictionary[sceneNum + "-" + eventName].dialogueRows;
    }

}

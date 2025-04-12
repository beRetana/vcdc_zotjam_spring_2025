using System;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    const int TOTAL_COLS = 9;

    public TextAsset textAssetData;

    [System.Serializable]
    public class DialogueRow
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
    }

    public enum CharacterEnum { None, Her, Me };
    public enum TypeEnum { Std, Q, R1, R2, R3, Wait };

    [System.Serializable]
    public class DialogueList
    {
        public DialogueRow[] dialogueRow;
    }

    public DialogueList myDialogueList = new DialogueList();

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / TOTAL_COLS - 1;
        Debug.Log($"Table size = {tableSize}");
        myDialogueList.dialogueRow = new DialogueRow[tableSize];

        for (int i = 0; i < tableSize; ++i)
        {
            myDialogueList.dialogueRow[i] = new DialogueRow();
            myDialogueList.dialogueRow[i].rowName = data[TOTAL_COLS * (i + 1)];
            myDialogueList.dialogueRow[i].scene = TryParseInt(data[TOTAL_COLS * (i + 1) + 1]);
            myDialogueList.dialogueRow[i].eventName = data[TOTAL_COLS * (i + 1) + 2];
            myDialogueList.dialogueRow[i].sectionIndex = TryParseInt(data[TOTAL_COLS * (i + 1) + 3]);
            myDialogueList.dialogueRow[i].type = (TypeEnum)Enum.Parse(typeof(TypeEnum), data[TOTAL_COLS * (i + 1) + 4]);
            myDialogueList.dialogueRow[i].messageIndex = TryParseInt(data[TOTAL_COLS * (i + 1) + 5]);
            myDialogueList.dialogueRow[i].character = (CharacterEnum)Enum.Parse(typeof(CharacterEnum), data[TOTAL_COLS * (i + 1) + 6]);
            myDialogueList.dialogueRow[i].dialogue = data[TOTAL_COLS * (i + 1) + 7];
            myDialogueList.dialogueRow[i].lovePoints = TryParseInt(data[TOTAL_COLS * (i + 1) + 8]);
        }
    }

    private int TryParseInt(string value)
    {
        int result;
        return int.TryParse(value, out result) ? result : 0;
    }

    private void Start()
    {
        ReadCSV();
    }

}

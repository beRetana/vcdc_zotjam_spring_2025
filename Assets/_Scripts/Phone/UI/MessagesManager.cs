using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] private MessagesTemplates playerMessageTemplates;
    [SerializeField] private MessagesTemplates wifeMessageTemplates;

    [Serializable]
    public struct MessagesTemplates
    {
        public Transform OneMidLine;
        public Transform OneLine;
        public Transform TwoLine;
        public Transform ThreeLine;
    }

    private Queue<Transform> messagesHistory;
    private const int MAX_MESSAGES = 12;
    private int messagesCount;

    void Start ()
    {
        messagesHistory = new Queue<Transform>();
    }

    public void SendMessage(CSVReader.DialogueRow dialogueRow)
    {
        switch (dialogueRow.character)
        {
            case CSVReader.CharacterEnum.Her:
                ComposeMessage(wifeMessageTemplates, dialogueRow.dialogue);
                break;
            case CSVReader.CharacterEnum.Me:
                ComposeMessage(playerMessageTemplates, dialogueRow.dialogue);
                break;
            case CSVReader.CharacterEnum.None:
                break;
        }
    }

    private void ComposeMessage(MessagesTemplates template, string content)
    {
        if(content.Length <= 11)
        {
            DisplayMessage(template.OneMidLine, content);
        }
        else if (content.Length <= 23)
        {
            DisplayMessage(template.OneMidLine, content);
        }
        else if (content.Length <= 46)
        {
            DisplayMessage(template.OneMidLine, content);
        }
        else if (content.Length <= 69)
        {
            DisplayMessage(template.OneMidLine, content);
        }
        else
        {
            DisplayMessage(template.OneMidLine, content.Substring(0, 69));
            ComposeMessage(template,content.Substring(69));
        }
    }

    private void DisplayMessage(Transform messageTemplateType, string content)
    {
        if (messagesCount >= MAX_MESSAGES)
            messagesHistory.Dequeue();
        else ++messagesCount;
        
        Transform newMessage = Instantiate(messageTemplateType, transform);
        newMessage.GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
        messagesHistory.Enqueue(newMessage);
    }

}

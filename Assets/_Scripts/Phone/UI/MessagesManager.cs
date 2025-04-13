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
            DisplayMessage(template.OneLine, content);
        }
        else if (content.Length <= 46)
        {
            DisplayMessage(template.TwoLine, content);
        }
        else if (content.Length <= 69)
        {
            DisplayMessage(template.ThreeLine, content);
        }
        else
        {
            DisplayMessage(template.ThreeLine, content.Substring(0, 69));
            ComposeMessage(template,content.Substring(69));
        }
    }

    private void DisplayMessage(Transform messages, string content)
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.MessageReceived,PlayerMovement.Instance.transform.position);

        Transform newMessage = Instantiate(messages, Vector3.zero, Quaternion.identity);
        newMessage.gameObject.SetActive(false);
        MessageUI messageUI= newMessage.GetComponent<MessageUI>();
        messageUI.UpdateText(content);

        while (messageUI.Size + messagesCount > MAX_MESSAGES)
        {
            Transform temp = messagesHistory.Dequeue();
            messagesCount -= temp.GetComponent<MessageUI>().Size;
            Destroy(temp.gameObject);
        }

        messagesCount += messageUI.Size;
        //Debug.Log(messagesCount);
        newMessage.SetParent(transform);
        newMessage.gameObject.SetActive(true);
        messagesHistory.Enqueue(newMessage);
    }

}

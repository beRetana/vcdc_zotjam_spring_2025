using UnityEngine;
using System.Collections.Generic;

public class MessageQueue : MonoBehaviour
{
    const float AVG_CHARACTERS_PER_SECOND = 6.9f;
    const float AVG_RESPONSE_TIMER = 6.9f;

    private PhoneManagerUI phoneManagerUI;

    public enum TimerEnum { None, Typing, Responding };
    TimerEnum timerEnum = TimerEnum.None;

    private LinkedList<MessageObject> _MessageQueue;
    public float messageTimer { get; private set; } = 0f;


    public enum TextEnum { Message, ResponseOptions};


    public class MessageObject
    {
        public TextEnum textEnum;
        public CSVReader.DialogueRow[] dialogueRows; //TK
        public float lengthInSeconds;

        public MessageObject (TextEnum textEnum, params CSVReader.DialogueRow[] dialogueRows)
        {
            this.textEnum = textEnum;
            this.dialogueRows = dialogueRows;

            if(textEnum == TextEnum.Message)
                lengthInSeconds = (float)dialogueRows[0].dialogue.Length / AVG_CHARACTERS_PER_SECOND;
            else if(textEnum == TextEnum.ResponseOptions)
                lengthInSeconds = AVG_RESPONSE_TIMER;
        }
    }

    private void ProcessMessaging()
    {
        if (_MessageQueue.Count == 0)
        {
            timerEnum = TimerEnum.None;
            return;
        }

        MessageObject currentMessage = _MessageQueue.First.Value;
        if (messageTimer <= 0f)
        {
            SendMessage();
            NextMessage();
            return;
        }
        else
        {
            messageTimer -= Time.fixedDeltaTime;
        }
    }


    private void SendMessage()
    {
        MessageObject currentMessage = _MessageQueue.First.Value;

        phoneManagerUI.DisplayMessage(currentMessage.dialogueRows[0]); //TK 
    }


    private void NextMessage()
    {

        if (_MessageQueue.Count > 0)
            _MessageQueue.RemoveFirst();

        if (_MessageQueue.Count == 0)
        {
            timerEnum = TimerEnum.None;
            return;
        }
        SetTimer();
    }




    private void Start()
    {
        phoneManagerUI = FindFirstObjectByType<PhoneManagerUI>();
        _MessageQueue = new LinkedList<MessageObject>();
        //typingSpeed = Time.fixedDeltaTime * AVG_CHARACTERS_PER_SECOND;
    }

    void FixedUpdate()
    {
        ProcessMessaging();
    }

    private void SetTimer()
    {
        MessageObject currentMessage = _MessageQueue.First.Value;
        if (currentMessage.textEnum == TextEnum.Message) timerEnum = TimerEnum.Typing;
        else if (currentMessage.textEnum == TextEnum.ResponseOptions) timerEnum = TimerEnum.Responding;

        messageTimer = currentMessage.lengthInSeconds;
    }


    public void QueueMessage(CSVReader.DialogueRow r)
    {
        MessageObject newMessage = new MessageObject(TextEnum.Message, r);
        _MessageQueue.AddLast(newMessage);

    }


    public void QueueResponseOptions(string[] responseChoices)
    {

    }

}

using UnityEngine;
using System.Collections.Generic;

public class MessageQueue : MonoBehaviour
{
    const float AVG_CHARACTERS_PER_SECOND = 10f;
    const float AVG_RESPONSE_TIMER = 6.9f;

    public bool AcceptingResponses { get; private set; } = false;

    private PhoneManagerUI phoneManagerUI;

    public enum TimerEnum { None, Typing, Responding };
    TimerEnum timerEnum = TimerEnum.None;

    private LinkedList<MessageObject> _MessageQueue;
    public float messageTimer { get; private set; } = 0f;
    public float countdownTimer { get; private set; } = 0f;


    public enum TextEnum { Wait, Message, ResponseOptions};


    private TextEnum previousTextEnum = TextEnum.Wait;

    public class MessageObject
    {
        public TextEnum textEnum;
        public CSVReader.DialogueRow dialogueRow; //TK
        public float lengthInSeconds;
        public string[] dialogueStrings;

        public MessageObject (CSVReader.DialogueRow dialogueRow, TextEnum textEnum = TextEnum.Message)
        {
            this.textEnum = textEnum;
            this.dialogueRow = dialogueRow;

            if(textEnum == TextEnum.Message)
                lengthInSeconds = (float)dialogueRow.dialogue.Length / AVG_CHARACTERS_PER_SECOND;
        }

        public MessageObject(string[] dialogueStrings, TextEnum textEnum = TextEnum.ResponseOptions)
        {
            this.textEnum = textEnum;
            this.dialogueStrings = dialogueStrings;

            if (textEnum == TextEnum.ResponseOptions)
                lengthInSeconds = AVG_RESPONSE_TIMER;
        }

        public MessageObject(float waitForSeconds, TextEnum textEnum = TextEnum.Wait)
        {
            this.textEnum = textEnum;

            if (textEnum == TextEnum.Wait)
                lengthInSeconds = waitForSeconds;
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
        //Debug.Log($"CurrentMessage of type {currentMessage.textEnum} | {messageTimer}");
        if (messageTimer <= 0f)
        {
            ExitMessage();
            NextMessage();

            if(_MessageQueue.Count > 0)
                EnterMessage();
            return;
        }
        else
        {
            messageTimer -= Time.fixedDeltaTime;
        }
    }
    


    private void NextMessage()
    {
        if (_MessageQueue.Count > 0)
        {
            previousTextEnum = _MessageQueue.First.Value.textEnum;
            _MessageQueue.RemoveFirst();
        }
    }

    private void EnterMessage()
    {
        MessageObject currentMessage = _MessageQueue.First.Value;

        switch (currentMessage.textEnum)
        {
            case TextEnum.ResponseOptions:
                EnterResponses(currentMessage.dialogueStrings);
                break;

            case TextEnum.Message:
                EnterStandardMessage(currentMessage.lengthInSeconds);
                break;

            case TextEnum.Wait:
                EnterWait(currentMessage.lengthInSeconds);
                break;
        }
    }

    /// <summary>
    /// called when timer hits 0
    /// </summary>
    private void ExitMessage()
    {
        MessageObject currentMessage = _MessageQueue.First.Value;

        switch (currentMessage.textEnum)
        {
            case TextEnum.ResponseOptions:
                ExitResponses(false); //this runs if ran out of time
                break;

            case TextEnum.Message:
                ExitStandardMessage(currentMessage.dialogueRow);
                break;

            case TextEnum.Wait:
                ExitWait();
                break;
        }
    }

    private void EnterWait(float waitTime)
    {
        timerEnum = TimerEnum.None;

        messageTimer = waitTime;
    }
    private void ExitWait()
    {
        previousTextEnum = TextEnum.Wait;
    }


    private void EnterStandardMessage(float typingTime)
    {
        timerEnum = TimerEnum.Typing;

        messageTimer = typingTime;
    }
    private void ExitStandardMessage(CSVReader.DialogueRow dialogueRow)
    {
        SendMessage(dialogueRow);
        previousTextEnum = TextEnum.Message;
    }
    private void SendMessage(CSVReader.DialogueRow dialogueRow)
    {
        phoneManagerUI.DisplayMessage(dialogueRow);
    }


    private void EnterResponses(string[] dialogueStrings)
    {
        timerEnum = TimerEnum.Responding;

        AcceptingResponses = true;

        phoneManagerUI.DisplayStringOptionsUI(dialogueStrings);

        messageTimer = AVG_RESPONSE_TIMER * 1.0f; //TK multiplier

        //if (_MessageQueue.Count > 1)
        //{
        //    messageTimer = 0f;
        //    NextMessage();
        //}
    }
    private void ExitResponses(bool selectedOption)
    {
        previousTextEnum = TextEnum.ResponseOptions;

        //if(!selectedOption)
            //TK Select Random Option
        phoneManagerUI.DisplayNoOptionsUI();
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
        Debug.Log($"[MessageQueue] previousTextEnum = {previousTextEnum} | currentMessage = {_MessageQueue.First.Value.textEnum} | queueCount = {_MessageQueue.Count}");


        MessageObject currentMessage = _MessageQueue.First.Value;
        if (currentMessage.textEnum == TextEnum.Message) timerEnum = TimerEnum.Typing;
        else if (currentMessage.textEnum == TextEnum.ResponseOptions) timerEnum = TimerEnum.Responding;

        messageTimer = currentMessage.lengthInSeconds;

        if (currentMessage.dialogueRow.character == CSVReader.CharacterEnum.Me) messageTimer = 0f;
    }


    public void QueueMessage(CSVReader.DialogueRow r)
    {
        if(_MessageQueue.Count == 0)
        {
            messageTimer = 0f;
        }

        MessageObject newMessage = new MessageObject(r, TextEnum.Message);
        Debug.Log($"Queuing {r.dialogue} at index {_MessageQueue.Count}");
        _MessageQueue.AddLast(newMessage);

        if(_MessageQueue.Count == 1)
        {
            SetTimer();
            //PlayCurrentMessage();
        }

    }


    public void QueueResponseOptions(string[] responseChoices)
    {
        MessageObject newMessage = new MessageObject(responseChoices, TextEnum.ResponseOptions);
        _MessageQueue.AddLast(newMessage);
        messageTimer = 0f;
        previousTextEnum = TextEnum.ResponseOptions;
        //NextMessage();
    }

    public void SelectedDialogue()
    {
        AcceptingResponses = false;
        ExitResponses(true);
        NextMessage();
        messageTimer = 0f;
        ProcessMessaging();
    }
}

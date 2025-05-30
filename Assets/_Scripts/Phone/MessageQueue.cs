using UnityEngine;
using System.Collections.Generic;

public class MessageQueue : MonoBehaviour
{
    const float AVG_CHARACTERS_PER_SECOND = 12f;
    const float AVG_ADDITIONAL_MESSAGE_TIME = .5f;
    const float AVG_RESPONSE_TIMER = 6.9f;

    public delegate void EventMessageCompletion();
    public static EventMessageCompletion OnEventMessageCompletion;

    public bool AcceptingResponses { get; private set; } = false;

    private PhoneManagerUI phoneManagerUI;
    [SerializeField] Love loveManager;
    [SerializeField] TimerUI timerUIManager;

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
        public CSVReader.DialogueRow[] responseChoices;

        public MessageObject (CSVReader.DialogueRow dialogueRow, TextEnum textEnum = TextEnum.Message)
        {
            this.textEnum = textEnum;
            this.dialogueRow = dialogueRow;

            if(textEnum == TextEnum.Message)
                lengthInSeconds = (float)dialogueRow.dialogue.Length / AVG_CHARACTERS_PER_SECOND;
        }

        public MessageObject(CSVReader.DialogueRow[] responseChoices, TextEnum textEnum = TextEnum.ResponseOptions)
        {
            this.textEnum = textEnum;
            this.responseChoices = responseChoices;

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

    private void OnEnable()
    {
        SceneTransition.OnSceneLoaded += FindObjectComponents;
    }

    private void FindObjectComponents()
    {
        loveManager = FindFirstObjectByType<Love>();
        timerUIManager = FindFirstObjectByType<TimerUI>();
    }

    private void Start()
    {
        phoneManagerUI = FindFirstObjectByType<PhoneManagerUI>();
        //loveManager = FindFirstObjectByType<Love>();

        _MessageQueue = new LinkedList<MessageObject>();

        phoneManagerUI.DisplayNoOptionsUI();
        //typingSpeed = Time.fixedDeltaTime * AVG_CHARACTERS_PER_SECOND;
    }

    void FixedUpdate()
    {
        ProcessMessaging();
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
            if(currentMessage.textEnum == TextEnum.ResponseOptions)
                timerUIManager.SetTime(messageTimer);
        }
    }



    private void NextMessage()
    {
        if (_MessageQueue.Count > 0)
        {
            previousTextEnum = _MessageQueue.First.Value.textEnum;
            _MessageQueue.RemoveFirst();

            if (_MessageQueue.Count == 0)
            {
                Debug.Log("DONE WITH EVENT");
                OnEventMessageCompletion?.Invoke();

            }
        }
    }
    private void EnterMessage()
    {
        MessageObject currentMessage = _MessageQueue.First.Value;

        switch (currentMessage.textEnum)
        {
            case TextEnum.ResponseOptions:
                EnterResponses(currentMessage.responseChoices);
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
        timerUIManager.StopTime();

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

        messageTimer = typingTime + AVG_ADDITIONAL_MESSAGE_TIME;
    }
    private void ExitStandardMessage(CSVReader.DialogueRow dialogueRow)
    {
        SendMessage(dialogueRow);
        previousTextEnum = TextEnum.Message;


        if (dialogueRow.lovePoints != 0)
           FindFirstObjectByType<Love>().editHerLove(dialogueRow.lovePoints);
        //else
          //  Debug.Log("No Love Manager");
    }
    private void SendMessage(CSVReader.DialogueRow dialogueRow)
    {
        phoneManagerUI.DisplayMessage(dialogueRow);
    }


    private void EnterResponses(CSVReader.DialogueRow[] dialogueStrings)
    {
        timerEnum = TimerEnum.Responding;

        AcceptingResponses = true;

        phoneManagerUI.DisplayOptionsUI(dialogueStrings);

        if (loveManager != null)
            messageTimer = 1.618f * Mathf.Log(loveManager.getLove() + 1.618f); //TK multiplier
        else
            messageTimer = AVG_RESPONSE_TIMER;
    }
    private void ExitResponses(bool selectedOption)
    {
        previousTextEnum = TextEnum.ResponseOptions;

        if(!selectedOption)
        {
            AcceptingResponses = false;
            GetWorstResponse();

            //EnterWait(_MessageQueue.First.Value.lengthInSeconds);
            //ExitStandardMessage(_MessageQueue.First.Value.dialogueRow);
            //EnterMessage();
                //EnterMessage();
            //ExitMessage();
            //ExitStandardMessage(_MessageQueue.First.Value.dialogueRow);
                //ExitResponses(true);
            //EnterMessage();
            //ExitStandardMessage(_MessageQueue.First.Value.dialogueRow);
            //ProcessMessaging();
            //AcceptingResponses = false;

            //ExitResponses(true);
            //NextMessage();
            //messageTimer = 0f;
            //ProcessMessaging();
        }
          //  _MessageQueue.First.Value.dialogueRow.sectionIndex

            //TK Select Random Option
        phoneManagerUI.DisplayNoOptionsUI();
    }

    private void GetWorstResponse()
    {
        CSVReader.DialogueRow[] responseChoices = _MessageQueue.First.Value.responseChoices;

        int love0 = responseChoices[0].lovePoints;
        int love1 = responseChoices[1].lovePoints;
        int love2 = responseChoices[2].lovePoints;

        CSVReader.TypeEnum worstType = CSVReader.TypeEnum.R3;
        if (love0 <= love1 && love0 <= love2) worstType = CSVReader.TypeEnum.R1;
        if (love1 <= love0 && love0 <= love2) worstType = CSVReader.TypeEnum.R2;

        FindFirstObjectByType<DialogueParser>().GetWorstDialogue(worstType);
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


    public void QueueResponseOptions(CSVReader.DialogueRow[] responseChoices)
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

    public void SelectedWorstDialogue()
    {
        AcceptingResponses = false;
        //ExitResponses(true);
        messageTimer = 0f;
        previousTextEnum = TextEnum.ResponseOptions;

        //ProcessMessaging();

        //ExitMessage();
        //ExitStandardMessage(_MessageQueue.First.Value.dialogueRow);
        //NextMessage();
        //messageTimer = 0f;
        //ProcessMessaging();
    }

}

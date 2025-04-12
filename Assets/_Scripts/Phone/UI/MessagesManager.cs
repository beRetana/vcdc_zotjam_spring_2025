using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    [SerializeField] private MessagesSizes playerMessagesSizes;

    [Serializable]
    public struct MessagesSizes
    {
        
    }

    private Queue messagesHistory;

    void Start ()
    {
        messagesHistory = new Queue();
    }


}

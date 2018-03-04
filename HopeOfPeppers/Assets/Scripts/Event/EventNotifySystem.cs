using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotifySystem
{
    private EventNotifySystem()
    {
    }
    
    private static EventNotifySystem _instance = null;
    public static EventNotifySystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EventNotifySystem();

            return _instance;
        }
    }

    public enum EVENT_ID : int 
    {
        GOLD_CHANGE,    //금화 변경.
        WORK_LEVELUP,   //워크 레벨업.
    }

    public delegate void EventNotifyCallback(Int64[] aEventValues);

    private Dictionary<EVENT_ID, EventNotifyCallback> dicEventNotifyCallbacks = new Dictionary<EVENT_ID, EventNotifyCallback>();


    public void AddEventCallback(EVENT_ID aEventID, EventNotifyCallback aCallback)
    {
        if (false == dicEventNotifyCallbacks.ContainsKey(aEventID))
            dicEventNotifyCallbacks.Add(aEventID, aCallback);
        else
            dicEventNotifyCallbacks[aEventID] += aCallback;
    }

    public void RemoveEventCallback(EVENT_ID aEventID, EventNotifyCallback aCallback)
    {
        dicEventNotifyCallbacks[aEventID] -= aCallback;
    }

    public void NotifyEvent(EVENT_ID aEventID, Int64[] aEventValues)
    {
        if (false == dicEventNotifyCallbacks.ContainsKey(aEventID))
            return;

        if (null == dicEventNotifyCallbacks[aEventID])
            return;

        dicEventNotifyCallbacks[aEventID](aEventValues);
    }





}

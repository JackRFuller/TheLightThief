using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoSingleton<EventManager>
{
    private Dictionary<string, UnityEvent> eventDictionary;

    private void Awake()
    {
        if(eventDictionary == null)
            eventDictionary = new Dictionary<string, UnityEvent>();
    }
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (Instance == null)
            return;

        UnityEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }



}

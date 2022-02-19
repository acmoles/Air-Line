using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringEvent", menuName = "Utils/StringEvent")]
public class StringEvent : ScriptableObject
{
    private string keyValue = string.Empty;

    private List<StringEventListener> listeners = new List<StringEventListener>();

    public string KeyValue
    {
        get { return keyValue; }
        private set { }
    }

    public void Trigger(string value)
    {
        keyValue = value;
        Debug.Log("Trigger " + name + " " + value);

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(StringEventListener subscriber)
    {
        listeners.Add(subscriber);
    }

    public void UnRegisterListener(StringEventListener subscriber)
    {
        listeners.Remove(subscriber);
    }
}

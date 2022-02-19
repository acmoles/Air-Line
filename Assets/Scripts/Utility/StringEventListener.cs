using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StringEventListener : MonoBehaviour
{
    public StringEvent Event;
    public StringEventPayload Response;

    [System.Serializable]
    public class StringEventPayload : UnityEvent<string> { }

    private void OnEnable()
    {
        Event?.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event?.UnRegisterListener(this);
    }

    public void OnEventRaised(string value)
    {
        Response?.Invoke(value);
    }
}

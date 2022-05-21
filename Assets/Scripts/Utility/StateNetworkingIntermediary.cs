using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateNetworkingIntermediary", menuName = "Utils/StateNetworkingIntermediary")]
public class StateNetworkingIntermediary : ScriptableObject
{
    [SerializeField]
    private bool logging = true;

    [SerializeField]
    private bool localTest = true;

    private string cachedBrushMessage = null;
    public delegate void BrushDataChangedDelegate(string message);
    public BrushDataChangedDelegate brushDataChanged;

    private string cachedPosRotMessage = null;
    public delegate void PosRotChangedDelegate(string message);
    public PosRotChangedDelegate posRotChanged;

    private string cachedCoordMessage = null;
    public delegate void CoordChangedDelegate(string message);
    public CoordChangedDelegate coordChanged;

    public void PostMessage(MessageType type, string message)
    {
        if (localTest)
        {
            PostLocal(type, message);
        }
        else
        {
            // Send by http POST
            // Websockets?
            if(logging) Debug.LogWarning("noop");
        }
    }

    private void PostLocal(MessageType type, string message)
    {
        switch (type)
        {
            case MessageType.BrushStyles:
                if(logging) Debug.Log("BrushStyles delegate invoked");
                cachedBrushMessage = message;
                brushDataChanged.Invoke(message);
                break;
            case MessageType.PosRot:
                //if(logging) Debug.Log("PosRot delegate invoked");
                cachedPosRotMessage = message;
                posRotChanged.Invoke(message);
                break;
            case MessageType.CoordSystem:
                if(logging) Debug.Log("Coord delegate invoked");
                cachedCoordMessage = message;
                coordChanged.Invoke(message);
                break;
        }
    }

}

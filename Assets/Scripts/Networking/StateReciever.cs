using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReciever : MonoBehaviour
{
    // TEMP to replace with values coming in over the network
    [SerializeField]
    private StateCopier incomingState = null;

    [SerializeField]
    private Transform transformToControl = null;

    [SerializeField]
    private BrushStyles myBrushStyles = null;

    [SerializeField]
    private StateNetworkginIntermediary server = null;


    void Update()
    {
        // TODO get network values
        transformToControl.position = incomingState.Position;
        transformToControl.rotation = incomingState.Rotation;
    }

    void GetMessage()
    {

    }
}

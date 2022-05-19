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

    // TODO get BrushStyles object

    void Update()
    {
        // TODO get network values
        transformToControl.position = incomingState.Position;
        transformToControl.rotation = incomingState.Rotation;
    }
}

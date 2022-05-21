using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    PosRot,
    BrushStyles,
    CoordSystem // TODO spatial anchors
}

public class StateCopier : MonoBehaviour
{
    public bool logging = false;

    [SerializeField]
    private float xOffset = 0.3f;

    // [SerializeField]
    // private Transform coordSystemToCopy = null;

    [SerializeField]
    private Transform transformToCopy = null;

    [SerializeField]
    private BrushStyles brushStylesToCopy = null;

    [SerializeField]
    private StateNetworkingIntermediary networkingService = null;

    private Vector3 lastPosition = Vector3.zero;
    private Quaternion lastRotation = Quaternion.identity;

    void Update()
    {
        // Potentially debounce this?
        SendPosRot();
    }

    public void OnBrushStylesChanged(string state)
    {
        // BrushStyles handles it's own serialization
        string brushStylesMessage = brushStylesToCopy.SerializeBrushStyles();
        networkingService.PostMessage(MessageType.BrushStyles, brushStylesMessage);
    }

    private PosRotMessage cachedOutgoingMessage = new PosRotMessage();
    public void SendPosRot()
    {
        if (
            transformToCopy.position == lastPosition
            && transformToCopy.rotation == lastRotation
        )
        {
            return;
        }

        lastPosition = transformToCopy.position;
        lastRotation = transformToCopy.rotation;

        // Pack PosRot for network transfer
        Vector3 modifiedPos = transformToCopy.position;
        modifiedPos.x += xOffset;
        cachedOutgoingMessage.pos = modifiedPos;
        cachedOutgoingMessage.rot = transformToCopy.rotation;

        string postBody = JsonUtility.ToJson(cachedOutgoingMessage);

        if (logging) Debug.Log("PosRot sent: " + postBody);
        networkingService.PostMessage(MessageType.PosRot, postBody);
    }
}

[Serializable]
public class PosRotMessage
{
    public Vector3 pos;
    public Quaternion rot;
}

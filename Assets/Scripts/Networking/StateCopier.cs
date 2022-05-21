using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCopier : MonoBehaviour
{
    public bool logging = false;

    [SerializeField]
    private float xOffset = 0.3f;

    [SerializeField]
    private Transform transformToCopy = null;

    [SerializeField]
    private BrushStyles brushStylesToCopy = null;

    // TEMP for testing
    public Vector3 Position
    {
        get
        {
            Vector3 position = transformToCopy.position;
            position.x += xOffset;
            return position;
        }
    }
    public Quaternion Rotation
    {
        get => transformToCopy.rotation;
    }

    public BrushStyles BrushStylesToMatch
    {
        get => brushStylesToCopy;
    }
    // END TEMP

    void Update()
    {
        // Potentially debounce this?
        SendPosRot();
    }

    public void OnBrushStylesChanged(string state)
    {
        // BrushStyles handles it's own serialization
        string brushStylesMessage = brushStylesToCopy.SerializeBrushStyles();
        PostMessage(brushStylesMessage);
    }

    public void SendPosRot()
    {
        // Serialize pos rot of transformToMatch
        // If values changed (like position reporter)
        // TODO send transform position and rotation POST
        PostMessage("PosRot");
    }

    public void PostMessage(string message)
    {
        // Send by POST
    }
}

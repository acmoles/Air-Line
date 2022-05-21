using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReciever : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    [SerializeField]
    private Transform transformToControl = null;

    [SerializeField]
    private BrushStyles brushStylesToControl = null;

    // [SerializeField]
    // private Transform coordSystemToControl = null;

    [SerializeField]
    private StateNetworkingIntermediary networkingService = null;

    private void OnEnable()
    {
        networkingService.brushDataChanged += OnBrushStylesChanged;
        networkingService.posRotChanged += OnPosRotChanged;
        networkingService.coordChanged += OnCoordChanged;
    }

    private void OnDisable()
    {
        networkingService.brushDataChanged -= OnBrushStylesChanged;
        networkingService.posRotChanged -= OnPosRotChanged;
        networkingService.coordChanged -= OnCoordChanged;
    }

    private void OnBrushStylesChanged(string message)
    {
        brushStylesToControl.DeSerializeBrushStyles(message);
    }

    private PosRotMessage cachedIncomingMessage = new PosRotMessage();
    private void OnPosRotChanged(string message)
    {
        JsonUtility.FromJsonOverwrite(message, cachedIncomingMessage);
        if(logging) Debug.Log("PosRot revieved: " + cachedIncomingMessage);
        transformToControl.position = cachedIncomingMessage.pos;
        transformToControl.rotation = cachedIncomingMessage.rot;
    }

    private void OnCoordChanged(string message)
    {
        Debug.LogWarning("noop");
    }
}

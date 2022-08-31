using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonVisual : ToggleResponder
{
    [SerializeField]
    private Toggle toggle = null;

    [SerializeField]
    private GameObject OffVisual = null;

    [SerializeField]
    private GameObject OnVisual = null;

    [SerializeField]
    private StringEvent toggleEvent = null;

    // [SerializeField]
    // private float transitionTime = 0.4f;


    //TODO animation
    protected override void SetVisual()
    {
        if (OnVisual != null) OnVisual.SetActive(toggle.isOn);
        if (OffVisual != null) OffVisual.SetActive(!toggle.isOn);
        if (toggleEvent != null) toggleEvent.Trigger(toggle.isOn.ToString());
    }
}

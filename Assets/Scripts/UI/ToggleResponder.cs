using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ToggleResponder : MonoBehaviour
{
    private bool setScheduled = false;

    private void Start()
    {
        SetVisual();
    }

    public void OnPress(bool toggle)
    {
        setScheduled = true;
    }

    private void LateUpdate()
    {
        if (setScheduled)
        {
            SetVisual();
            setScheduled = false;
        }
    }

    protected virtual void SetVisual()
    {

    }
}

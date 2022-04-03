using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    StringEvent onToggleBrushUpDown;

    [SerializeField]
    StringEvent onToggleFollowMe;

    [SerializeField]
    Transform target;

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        { }
    }

    [SerializeField]
    WaypointManager waypointManager;

    [SerializeField]
    BrushStyles brushStyles;

    public void OnPlace()
    {
        waypointManager.AddPoint(target.transform.position, brushStyles.BrushColor);
    }

    public void OnSize(BrushSize size)
    {
        brushStyles.BrushSize = size;
    }

    public void OnColor(BrushColor color)
    {
        brushStyles.BrushColor = color;
    }

    public void OnToggleFollowMe(string toggle)
    {
        onToggleFollowMe.Trigger(toggle);
    }

    public void OnToggleBrushUpDown(string toggle)
    {
        onToggleBrushUpDown.Trigger(toggle);
    }
}

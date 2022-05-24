using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
/////UI events to hook up/////
- Place waypoint

- Toggle brush up/down (end/start line)
- Change brush size
- Change colour
- Replay sequence (on new line?)
- Follow moving target
- Play pause
- Debug field for max tube points
*/

[SelectionBase]
public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    StringEvent movementStateUpdated;

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

    public void OnToggleMovementState(string toggle)
    {
        movementStateUpdated.Trigger(toggle);
    }

    public void OnToggleBrushUpDown(BrushUpDownState toggle)
    {
        brushStyles.BrushToggle = toggle;
    }
}

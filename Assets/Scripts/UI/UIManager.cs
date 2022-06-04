using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
/////UI events to hook up/////
- Place waypoint -
- Follow moving target -

Main UI
- Toggle brush up/down (end/start line)
- Change brush size
- Change colour
- Play pause

Top UI
- Toggle sequence list
- (Sequence list item) play sequence
- (Text) name of last sequence played, what default text?

Debug UI
- ----Replay sequence (on new line?)----
- Debug field for max tube points
*/

public class UIManager : MonoBehaviour
{
    [SerializeField]
    protected StringEvent movementStateUpdated = null;

    [SerializeField]
    protected BrushStyles brushStyles = null;


    // Brush size
    public int brushSizeIndex = 0;
    public void OnSize(BrushSize size)
    {
        brushStyles.BrushSize = size;
    }
    public void OnSize(bool toggle)
    {
        brushSizeIndex++;
        if (brushSizeIndex >= Enum.GetNames(typeof(BrushSize)).Length)
        {
            brushSizeIndex = 0;
        }
        OnSize((BrushSize)brushSizeIndex);
    }

    // Brush color
    public void OnColor(BrushColor color)
    {
        brushStyles.BrushColor = color;
    }
    public void OnChangeColor(string message)
    {
        BrushColor converted;
        if(Enum.TryParse<BrushColor>(message, out converted))
        {
            OnColor(converted);
        }
        else
        {
            Debug.LogWarning("Not a BrushColor: " + message);
        }
    }

    // Movement state (play/pause)
    public void OnToggleMovementState(TurtleMovementState toggle)
    {
        movementStateUpdated.Trigger(toggle.ToString());
    }
    public void OnToggleMovementState(bool toggle)
    {
        if (toggle) OnToggleMovementState(TurtleMovementState.Play);
        else OnToggleMovementState(TurtleMovementState.Pause);
    }

    // Brush up/down
    public void OnToggleBrushUpDown(BrushUpDownState toggle)
    {
        brushStyles.BrushToggle = toggle;
    }
    public void OnToggleBrushUpDown(bool toggle)
    {
        if (toggle) OnToggleBrushUpDown(BrushUpDownState.Down);
        else OnToggleBrushUpDown(BrushUpDownState.Up);
    }
}

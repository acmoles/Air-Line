using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
- Screen z offset
*/

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    [SerializeField]
    private StringEvent movementStateUpdated = null;

    [SerializeField]
    private BrushStyles brushStyles = null;

    // Initial state:
    // Brush is set to down on start in BrushStyles
    // Play is set in Turtle on start


    // Brush size
    public int BrushSizeIndex
    {
        set
        {
            if (value >= Enum.GetNames(typeof(BrushSize)).Length)
            {
                value = 0;
            }
            OnSize((BrushSize)value);
        }
        get
        {
            return (int)brushStyles.BrushSize;
        }
    }
    public void OnSize(BrushSize size)
    {
        brushStyles.BrushSize = size;
    }
    public void OnSize(bool toggle)
    {
        BrushSizeIndex++;
    }

    // Brush color
    public void OnColor(BrushColor color)
    {
        brushStyles.BrushColor = color;
    }
    public void OnChangeColor(string message)
    {
        BrushColor converted;
        if (Enum.TryParse<BrushColor>(message, out converted))
        {
            if (logging) Debug.Log("New brush color: " + converted);
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

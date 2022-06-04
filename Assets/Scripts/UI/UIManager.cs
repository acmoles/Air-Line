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

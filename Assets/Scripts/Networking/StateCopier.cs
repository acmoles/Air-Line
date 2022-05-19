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
        get {
            Vector3 position = transformToCopy.position;
            position.x += xOffset;
            return position;
        }
    }
    public Quaternion Rotation
    {
        get => transformToCopy.rotation;
    }
    // END TEMP

    void Update()
    {
        // TODO send transform position and rotation POST
        // Potentially debounce this?
    }

    // TODO remove this in Turtle too
    // Turtle shouldn't own this - but can control it through script
    public void OnToggleBrushDown(string state)
    {
        if (logging) Debug.Log("State: " + state);
        if (state == BrushUpDownState.Up.ToString())
        {
            if (logging) Debug.Log("Set brush up networking");
            // TODO send POST brush up
        }
        else if (state == BrushUpDownState.Down.ToString())
        {
            if (logging) Debug.Log("Set brush down networking");
            // TODO send POST brush down
        }
    }

    // Switch to using this event for brush up/down
    public void OnBrushStylesChanged(BrushStyles state)
    {

    }
}

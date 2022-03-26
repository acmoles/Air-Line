using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    WaypointManager waypointManager;

    [SerializeField]
    BrushStyles brushStyles;

    public void OnPlace() {
        waypointManager.AddPoint(transform.position);
    }

    public void OnSize(BrushSize size) {
        brushStyles.BrushSize = size;
    }

    public void OnColor(Color color) {
        brushStyles.Color = color;
    }

    public void OnToggleSnap(bool toggle) {
        //TODO is snapping
        //where this state?
    }
}

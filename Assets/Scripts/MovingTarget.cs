using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    WaypointManager waypointManager;

    [SerializeField]
    StyleReporter styleReporter;

    public void OnPlace() {
        waypointManager.AddPoint(transform.position);
    }

    public void OnSize() {
        styleReporter.BrushSize = BrushSize.Large;
    }

    public void OnColor() {
        styleReporter.Color = Color.red;
    }
}

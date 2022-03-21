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

    public void OnSize(BrushSize size) {
        styleReporter.BrushSize = size;
    }

    public void OnColor(Color color) {
        styleReporter.Color = color;
    }
}

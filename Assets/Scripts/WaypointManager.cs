using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    StringEvent updatedEvent;


    [SerializeField, HideInInspector]
    bool initialized;

    public List<Vector3> points {
        get
        {
            if (_points == null)
            {
                _points = new List<Vector3>();
            }
            return _points;
        }
    }

    [SerializeField, HideInInspector]
    List<Vector3> _points;

    public Transform[] waypoints;


    void Start()
    {
        if (waypoints.Length > 0)
        {
            // Do something with list of waypoints
            for (int i = 0; i < points.Count; i++) {
                AddPoint(waypoints[i].position);
            }
        }
    }

    public void AddPoint(Vector3 point) {
        points.Add(point);
        updatedEvent.Trigger("update");
    }

//TODO able to add waypoints in-game

#if UNITY_EDITOR
    void OnDrawGizmos () {
        Gizmos.color = Color.green;
        for (int i = 0; i < points.Count; i++) {
            Gizmos.DrawWireSphere(points[i], .2f);
        }
    }

#endif

}

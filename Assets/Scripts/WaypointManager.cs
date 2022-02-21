using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Waypoint {
    public bool played;
    public Vector3 position;

    public  Waypoint(Vector3 position)
    {
        this.position = position;
        this.played = false;
    }
}


[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    StringEvent updatedEvent;


    [SerializeField, HideInInspector]
    bool initialized;

    public List<Waypoint> points {
        get
        {
            if (_points == null)
            {
                _points = new List<Waypoint>();
            }
            return _points;
        }
    }

    [SerializeField, HideInInspector]
    List<Waypoint> _points;

    public Transform[] targets;


    void Start()
    {
        if (targets.Length > 0)
        {
            // Do something with list of waypoints
            for (int i = 0; i < targets.Length; i++) {
                AddPoint(targets[i].position);
            }
        }
    }

    public void AddPoint(Vector3 position) {
        var point = new Waypoint(position);
        points.Add(point);
        updatedEvent.Trigger("update");
    }

//TODO able to add waypoints in-game

#if UNITY_EDITOR
    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++) {
            Gizmos.DrawWireSphere(points[i].position, .2f);
        }
    }

#endif

}

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

    [SerializeField]
    WaypointVisual waypointVisual;


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
            for (int i = 0; i < targets.Length; i++) {
                // add to points list without triggering update event
                var point = new Waypoint(targets[i].position, BrushColor.Orange);
                points.Add(point);
            }
        }
    }

    public void AddPoint(Vector3 position, BrushColor color) {
        //TODO check if new waypoint is within close threshhold of previous waypoint
        var point = new Waypoint(position, color);
        points.Add(point);
        updatedEvent.Trigger("update");

        WaypointVisual visual = Instantiate(waypointVisual, position, Quaternion.identity);
        point.visual = visual;
        visual.SetColor(color);
        point.AnimateInVisual();
    }

#if UNITY_EDITOR
    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++) {
            Gizmos.DrawWireSphere(points[i].position, .2f);
        }
    }

#endif

}

public class Waypoint {
    public bool played;
    public bool next;
    public Vector3 position;
    public BrushColor color;
    public WaypointVisual visual;

    public Waypoint(Vector3 position, BrushColor color)
    {
        this.position = position;
        this.played = false;
        this.next = false;
        this.color = color;
    }

    public void AnimateInVisual() {
        if(visual != null) visual.AnimateIn();
    }

    public void AnimateOutVisual() {
        Debug.Log("animate out visual");
        if(visual != null) visual.AnimateOut();
    }
}

using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    [SerializeField]
    StringEvent updatedEvent;

    [SerializeField]
    WaypointVisual waypointVisual;

    [SerializeField]
    AnimationSettings animationSettings;


    [SerializeField, HideInInspector]
    bool initialized;

    [SerializeField]
    public List<Waypoint> points
    {
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
            for (int i = 0; i < targets.Length; i++)
            {
                // add to points list without triggering update event
                var point = new Waypoint(targets[i].position, BrushColor.Orange);
                points.Add(point);
            }
        }
    }

    public void AddPoint(Vector3 position, BrushColor color)
    {
        //TODO check if new waypoint is within close threshhold of previous waypoint
        var point = new Waypoint(position, color);
        points.Add(point);
        updatedEvent.Trigger("update");

        WaypointVisual visual = Instantiate(waypointVisual, position, Quaternion.identity);
        point.visual = visual;
        visual.SetColor(color);
        if (points[points.Count - 2].played)
        {
            if(logging) Debug.Log("New point is active");
            point.visual.SetNext(points.Count - 1);
        }
        point.visual.AnimateIn();
    }

    public bool WaypointsToPlay()
    {
        foreach (var waypoint in points)
        {
            if (!waypoint.played) return true;
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawWireSphere(points[i].position, .2f);
        }
    }

#endif

}

public class Waypoint
{
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
}

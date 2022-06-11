using System;
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
    StringEvent updatedEvent = null;

    [SerializeField]
    WaypointVisual waypointVisual = null;

    [SerializeField]
    BrushStyles brushStyles = null;

    private WaypointSingleton waypointSingleton = null;


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

    public List<Transform> targets = new List<Transform>();


    void Start()
    {
        waypointSingleton = WaypointSingleton.Instance;
        waypointSingleton.AddWaypointManager(this);

        if (targets.Count > 0)
        {
            System.Array values = System.Enum.GetValues(typeof(BrushColor));

            for (int i = 0; i < targets.Count; i++)
            {
                // add to points list without triggering update event
                BrushColor randomColor = (BrushColor)values.GetValue(UnityEngine.Random.Range(0, values.Length - 1));
                var point = new Waypoint(targets[i].position, randomColor);
                points.Add(point);
            }
        }
    }

    public void AddPoint(Vector3 position)
    {
        //TODO check if new waypoint is within close threshhold of previous waypoint
        var point = new Waypoint(position, brushStyles.BrushColor);
        points.Add(point);
        updatedEvent.Trigger("update");

        WaypointVisual visual = Instantiate(waypointVisual, position, Quaternion.identity);
        visual.transform.parent = transform;
        point.visual = visual;
        visual.SetColor(brushStyles.BrushColor);

        // If the waypoint before the one we just added is played
        if ((points[points.Count - 2] != null && points[points.Count - 2].played) || points.Count == 1)
        {
            // The one we just added is next to play
            if (logging) Debug.Log("New point is active");
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

[Serializable]
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

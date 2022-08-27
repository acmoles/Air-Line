using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MovingTarget : UIManager
{
    [SerializeField]
    Transform target;

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        { }
    }

    private WaypointSingleton waypointSingleton = null;

    private void Start()
    {
        waypointSingleton = WaypointSingleton.Instance;
    }

    public void OnPlace()
    {
        waypointSingleton.LocalManager.AddPoint(target.transform.position);
    }

    public void OnPlaceFake()
    {
        waypointSingleton.FakeManager.AddPoint(target.transform.position);
    }

    public void OnPlayFake()
    {
        waypointSingleton.FakeManager.NextWaypointSingle();
    }
}

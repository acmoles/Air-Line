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

    [SerializeField]
    WaypointManager waypointManager;

    public void OnPlace()
    {
        waypointManager.AddPoint(target.transform.position);
    }
}

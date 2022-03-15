using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    WaypointManager waypointManager;

    public void OnPlace() {
        waypointManager.AddPoint(transform.position);
    }
}

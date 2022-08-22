using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSingleton : Singleton<WaypointSingleton>
{
    [SerializeField]
    private bool logging = false;

    private List<WaypointManager> managers = new List<WaypointManager>();

    public WaypointManager LocalManager
    {
        get
        {
            if (managers.Count > 0)
            {
                return managers[0];
            }
            else
            {
                if(logging) Debug.Log("No waypoint managers added to singleton, not placing waypoint.");
                return null;
            }
        }
    }

    public WaypointManager FakeManager
    {
        get
        {
            if (managers.Count > 0)
            {
                for (int i = 0; i < managers.Count; i++)
                {
                    if(managers[i].FakeManager) return managers[i];
                }
                if(logging) Debug.Log("No fake managers added to singleton, not placing waypoint.");
                return null;
            }
            else
            {
                if(logging) Debug.Log("No waypoint managers added to singleton, not placing waypoint.");
                return null;
            }
        }
    }

    public void AddWaypointManager(WaypointManager manager)
    {
        managers.Add(manager);
    }
}

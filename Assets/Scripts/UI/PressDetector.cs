using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressDetector : MonoBehaviour
{
    [SerializeField]
    private Transform spawnable = null;

    [SerializeField]
    private float zOffset = 0.02f;

    private InputManager inputManager = null;

    private bool shouldSpawnWaypoint = true;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += Spawn;
        inputManager.OnHold += SetWaypointSpawnable;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Spawn;
        inputManager.OnHold -= SetWaypointSpawnable;
    }

    private void Spawn(Vector3 position, float time)
    {
        //TODO disallow touch on the bottom (UI) portion of the screen
        //Get current 2D touch position and ignore if within black-zone
        Debug.Log("End " + position);
        if (shouldSpawnWaypoint)
        {
            position.z += zOffset;
            //TODO properly create waypoints and ensure parented to contentParent
            Transform instance = Instantiate(spawnable, position, Quaternion.identity);
            instance.GetComponent<WaypointVisual>().AnimateIn();
            instance.parent = transform;
        }
    }

    private void SetWaypointSpawnable(bool isHeld)
    {
        shouldSpawnWaypoint = !isHeld;
    }
}

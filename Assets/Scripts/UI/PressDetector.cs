using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressDetector : MonoBehaviour
{
    [SerializeField]
    private Transform spawnable = null;

    [SerializeField]
    WaypointManager waypointManager = null;

    [SerializeField]
    private float zOffset = 0.02f;

    [SerializeField]
    private int screenYDeadzone = 100;

    [SerializeField]
    private int screenYDrawerToggledDeadzone = 200;
    private bool useToggledDeadzone = false;

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
        Vector2 screenPosition = inputManager.PrimaryPosition2D();
        int deadzone = useToggledDeadzone ? screenYDrawerToggledDeadzone : screenYDeadzone;
        if (screenPosition.y < deadzone)
        {
            return;
        }

        position.z += zOffset;

        if (shouldSpawnWaypoint)
        {
            if (waypointManager != null)
            {
                waypointManager.AddPoint(position);
            }
            else
            {
                Transform instance = Instantiate(spawnable, position, Quaternion.identity);
                instance.GetComponent<WaypointVisual>().AnimateIn();
                instance.parent = transform;
            }
        }
    }

    private void SetWaypointSpawnable(bool isHeld)
    {
        shouldSpawnWaypoint = !isHeld;
    }

    public void SetToggledDeadzone(string message)
    {
        bool messageBool;

        if (bool.TryParse(message, out messageBool))
        {
            useToggledDeadzone = messageBool;
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }
}

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

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += Spawn;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Spawn;
    }

    public void Spawn(Vector3 position, float time)
    {
        //TODO disallow touch on the bottom (UI) portion of the screen
        Debug.Log("End " + position);
        position.z += zOffset;
        Transform instance = Instantiate(spawnable, position, Quaternion.identity);
        instance.GetComponent<WaypointVisual>().AnimateIn();
        instance.parent = transform;
    }
}

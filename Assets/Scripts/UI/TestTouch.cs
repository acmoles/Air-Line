using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    [SerializeField]
    private Transform spawnable = null;

    [SerializeField]
    private float zOffset = -0.5f;

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

    public void Spawn(Vector2 screenPosition, float time)
    {
        Debug.Log("End " + screenPosition);
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane + zOffset);
        Vector3 worldCoordinates = Camera.main.ScreenToWorldPoint(screenCoordinates);
        Transform instance = Instantiate(spawnable, worldCoordinates, Quaternion.identity);
        instance.GetComponent<WaypointVisual>().AnimateIn();
        instance.parent = transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressDetector : MonoBehaviour
{
    [SerializeField]
    private Transform spawnable = null;

    private WaypointManager waypointManager = null;

    [SerializeField]
    private int screenYDeadzone = 100;

    [SerializeField]
    private BrushStyles brushStyles = null;

    [SerializeField]
    private int screenYDrawerToggledDeadzone = 200;
    private bool useToggledDeadzone = false;

    private InputManager inputManager = null;

    private bool shouldSpawnWaypoint = true;


    [Header("Debug")]
    [SerializeField]
    private RectTransform deadzoneVisual = null;

    [SerializeField]
    private GameObject tapVisual = null;

    [SerializeField]
    private GameObject noTapVisual = null;

    [SerializeField]
    private float tapDuration = 1.0f;
    private float tapTimer = 0.0f;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        //waypointManager = WaypointManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += Spawn;
        inputManager.OnHold += SetWaypointSpawnable;

        //TODO debug
        deadzoneVisual.anchoredPosition = new Vector2(deadzoneVisual.anchoredPosition.x, GetDeadzone());
        tapVisual.SetActive(false);
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Spawn;
        inputManager.OnHold -= SetWaypointSpawnable;
    }

    private void Spawn(Vector2 position, float time)
    {
        tapTimer = 0.0f;

        if (position.y < GetDeadzone())
        {
            noTapVisual.SetActive(true);
            return;
        }

        tapVisual.SetActive(true);

        Vector3 p = TouchUtils.ScreenToWorld(position, brushStyles.waypointScreenOffset);

        if (shouldSpawnWaypoint)
        {
            if (waypointManager != null)
            {
                waypointManager.AddPoint(p);
            }
            else
            {
                Transform instance = Instantiate(spawnable, p, Quaternion.identity);
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
            deadzoneVisual.anchoredPosition = new Vector2(deadzoneVisual.anchoredPosition.x, GetDeadzone());
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }

    private float GetDeadzone()
    {
        return useToggledDeadzone ? screenYDrawerToggledDeadzone : screenYDeadzone;
    }

    //TODO debug
    private void Update()
    {
        if (tapTimer < 1.0f)
        {
            tapTimer += Time.deltaTime / tapDuration;
            tapVisual.transform.position = inputManager.PrimaryPosition2D();
            noTapVisual.transform.position = inputManager.PrimaryPosition2D();
        }
        else
        {
            tapVisual.SetActive(false);
            noTapVisual.SetActive(false);
        }
    }
}

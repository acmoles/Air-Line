using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PressDetector : MonoBehaviour
{
    // [SerializeField]
    // private Transform spawnable = null;

    [SerializeField]
    private BrushStyles brushStyles = null;

    [SerializeField]
    private int screenYDeadzone = 100;

    [SerializeField]
    private int screenYDrawerToggledDeadzone = 200;
    private bool useToggledDeadzone = false;

    private InputManager inputManager = null;

    private bool shouldSpawnWaypoint = true;


    [Header("Debug")]

    [SerializeField]
    private GameObject tapVisual = null;

    [SerializeField]
    private GameObject noTapVisual = null;

    [SerializeField]
    private RectTransform deadzone = null;

    [SerializeField]
    private float tapDuration = 1.0f;
    private float tapTimer = 0.0f;
    private bool pointerOverUI = false;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += Spawn;
        inputManager.OnHold += SetWaypointSpawnable;

        //Debug
        tapVisual.SetActive(false);
        noTapVisual.SetActive(false);
        deadzone.sizeDelta = new Vector2(0f, useToggledDeadzone ? screenYDrawerToggledDeadzone : screenYDeadzone);
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Spawn;
        inputManager.OnHold -= SetWaypointSpawnable;
    }

    private void Spawn(Vector2 position, float time)
    {
        tapTimer = 0.0f;

        if (GetDeadzone().Contains(position))
        {
            noTapVisual.SetActive(true);
            return;
        }

        if (pointerOverUI)
        {
            noTapVisual.SetActive(true);
            Debug.Log("UI element blocked spawn");
            return;
        }

        tapVisual.SetActive(true);

        Vector3 p = TouchUtils.ScreenToWorld(position, brushStyles.waypointScreenOffset);

        if (shouldSpawnWaypoint)
        {
            if (WaypointSingleton.Instance.LocalManager != null)
            {
                WaypointSingleton.Instance.LocalManager.AddPoint(p);
            }
            // else
            // {
            //     Transform instance = Instantiate(spawnable, p, Quaternion.identity);
            //     instance.GetComponent<WaypointVisual>().AnimateIn();
            //     instance.parent = transform;
            // }
        }
    }

    // private void OnGUI()
    // {
    //     GUI.Box(GetDeadzone(), "");
    // }

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
            deadzone.sizeDelta = new Vector2(0f, useToggledDeadzone ? screenYDrawerToggledDeadzone : screenYDeadzone);
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }

    private Rect GetDeadzone()
    {
        return new Rect(0, 0, Screen.width, useToggledDeadzone ? screenYDrawerToggledDeadzone : screenYDeadzone);
    }

    private void Update()
    {
        pointerOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        //TODO debug
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

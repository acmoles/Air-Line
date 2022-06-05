using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class SimulatedPlayerInputManager : Singleton<SimulatedPlayerInputManager>
{
    [SerializeField]
    private bool logging = false;

    [SerializeField]
    private bool viewToggled = false;

    private SimulatedPlayerInput playerInput = null;

    private Camera mainCamera = null;

    public delegate void ToggleLookEvent(bool toggle);
    public event ToggleLookEvent OnToggleLook;

    private void Awake()
    {
        playerInput = new SimulatedPlayerInput();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        //TODO is this a Unity bug?
        //started and canceled both fire on keydown
        // playerInput.CameraControls.Toggle.started += ctx => ToggleOnLook(ctx);
        // playerInput.CameraControls.Toggle.canceled += ctx => ToggleOffLook(ctx);
        playerInput.CameraControls.Toggle.performed += ctx => ToggleLook(ctx);
    }

    private void ToggleLook(InputAction.CallbackContext ctx)
    {
        viewToggled = !viewToggled;
        if (logging) Debug.Log("View toggled: " + viewToggled);
        if (OnToggleLook != null) OnToggleLook(viewToggled);

        if(viewToggled) TouchSimulation.Disable();
        else TouchSimulation.Enable();
    }

    private void ToggleOnLook(InputAction.CallbackContext ctx)
    {
        if (logging) Debug.Log("Toggle on look");
        TouchSimulation.Disable();
        if (OnToggleLook != null) OnToggleLook(true);
    }

    private void ToggleOffLook(InputAction.CallbackContext ctx)
    {
        if (logging) Debug.Log("Toggle off look");
        TouchSimulation.Enable();
        if (OnToggleLook != null) OnToggleLook(false);
    }

    public Vector2 GetMovement()
    {
        return playerInput.CameraControls.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerInput.CameraControls.Look.ReadValue<Vector2>();
    }
}

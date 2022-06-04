using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class SimulatedPlayerInputManager : Singleton<SimulatedPlayerInputManager>
{
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
        playerInput.CameraControls.ToggleLook.started += ctx => ToggleOnLook(ctx);
        playerInput.CameraControls.ToggleLook.canceled += ctx => ToggleOffLook(ctx);
    }

    private void ToggleOnLook(InputAction.CallbackContext ctx)
    {
        if (OnToggleLook != null) OnToggleLook(true);
    }

    private void ToggleOffLook(InputAction.CallbackContext ctx)
    {
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

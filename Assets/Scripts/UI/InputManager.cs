using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private bool logging = false;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;


    public delegate void HoldEvent(bool toggle);
    public event HoldEvent OnHold;


    private TouchInput touchInput = null;

    private void Awake()
    {
        touchInput = new TouchInput();
    }

    private void OnEnable()
    {
        if (Application.isPlaying) touchInput.Enable();
        if (Application.isEditor) TouchSimulation.Enable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        if (Application.isPlaying) touchInput.Disable();
        if (Application.isEditor) TouchSimulation.Disable();

        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        touchInput.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchInput.Touch.TouchPress.canceled += ctx => EndTouch(ctx);

        touchInput.Touch.TouchHold.performed += ctx => StartHold(ctx);
        touchInput.Touch.TouchHold.canceled += ctx => EndHold(ctx);
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null) OnStartTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        if (logging) Debug.Log("Touch ended");
        if (OnEndTouch != null) OnEndTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }

    private void StartHold(InputAction.CallbackContext ctx)
    {
        if (logging) Debug.Log("Hold started");
        if (OnHold != null) OnHold(true);
    }

    private void EndHold(InputAction.CallbackContext ctx)
    {
        if (logging) Debug.Log("Hold ended");
        if (OnHold != null) OnHold(false);
    }

    // Direct finger API
    // private void FingerDown(Finger finger)
    // {
    //     if (OnStartTouch != null) OnStartTouch(TouchUtils.ScreenToWorld(mainCamera, finger.screenPosition), Time.time);
    // }

    public Vector3 PrimaryPosition()
    {
        return TouchUtils.ScreenToWorld(touchInput.Touch.TouchPosition.ReadValue<Vector2>());
    }

    public Vector3 PrimaryPosition2D()
    {
        return touchInput.Touch.TouchPosition.ReadValue<Vector2>();
    }

}

public static class TouchUtils
{
    public static Camera mainCamera
    {
        get => Camera.main;
    }
    public static Vector3 ScreenToWorld(Vector2 position, float zOffset = 0.0f)
    {
        Vector3 screenCoordinates = new Vector3(position.x, position.y, mainCamera.nearClipPlane + zOffset);
        return mainCamera.ScreenToWorldPoint(screenCoordinates);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
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
        //touchInput.Touch.TouchHold
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null) OnStartTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Touch ended");
        if (OnEndTouch != null) OnEndTouch(touchInput.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }

    private void FingerDown(Finger finger)
    {
        if (OnStartTouch != null) OnStartTouch(finger.screenPosition, Time.time);
    }
}

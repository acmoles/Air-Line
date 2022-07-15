using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPauseIndicator : MonoBehaviour
{
    [SerializeField]
    private Material material = null;

    private float time = 0f;
    private bool counting = true;


    void Update()
    {
        if(counting)
        {
            time += Time.deltaTime;
            material.SetFloat("_Timer", time);
        }
    }

    public void ToggleTimer(string state)
    {
        TurtleMovementState tState;
        if (Enum.TryParse<TurtleMovementState>(state, out tState))
        {
            switch (tState)
            {
                case TurtleMovementState.FollowMe:
                case TurtleMovementState.ExitFollowMe:
                case TurtleMovementState.Play:
                    counting = true;
                    break;
                case TurtleMovementState.Pause:
                    counting = false;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Cannot parse movement state");
        }
    }
}

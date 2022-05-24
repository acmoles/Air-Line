using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TurtleMovementState
{
    FollowMe,
    ExitFollowMe,
    Pause,
    Play
}

[CreateAssetMenu(fileName = "TurtleSettings", menuName = "Utils/TurtleSettings")]
public class TurtleSettings : ScriptableObject
{
    [Header("Turtle movement settings")]

    //public float moveTime = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public const float instantRotateSpeed = -1;
    public int arcSegments = 8;


    [Header("FollowMe Target Offset")]
    public float offset = 0.1f;
    public float offsetMovementThreshhold = 0.001f;


    [Header("FollowMe Speed")]
    public float speed = 50f;
    public float speedLimit = 100f;
    public float force = 1.5f;


    [Header("FollowMe Rotation")]
    public float followMeRotateSpeed = 0.5f;


    [Header("FollowMe Hover")]
    public float hoverFrequency = 1f;
    public float hoverAmount = 0.01f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "MovementSettings", menuName = "Utils/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    [Header("Target Offset")]
    public float offset = .1f;
    public float offsetMovementThreshhold = .01f;


    [Header("Speed")]
    public float speed = 4f;
    public float speedLimit = 4f;
    public float force = 1f;


    [Header("Rotation")]
    public float rotateSpeed = 0.5f;


    [Header("Hover")]
    public float hoverFrequency = 1f;
    public float hoverAmount = .3f;
}

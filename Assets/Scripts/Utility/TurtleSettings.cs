using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BrushUpDownState
{
    Up,
    Down
}

[CreateAssetMenu(fileName = "TurtleSettings", menuName = "Utils/TurtleSettings")]
public class TurtleSettings : ScriptableObject
{
    [Header("Turtle settings")]

    //public float moveTime = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public const float instantRotateSpeed = -1;
    public int arcSegments = 8;
}

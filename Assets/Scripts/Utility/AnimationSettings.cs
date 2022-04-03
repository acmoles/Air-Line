using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AnimationSettings", menuName = "Utils/AnimationSettings")]
public class AnimationSettings : ScriptableObject
{
    [Header("Waypoint animate in")]
    public float delay = 0.4f;
    public float animationDuration = 0.4f;
    public float finishedThreshold = 0.99f;
    public AnimationCurve inAnimationCurve;
    public AnimationCurve outAnimationCurve;

}

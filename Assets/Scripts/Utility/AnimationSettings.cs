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
    public float animationElasticAmp = 1f;
    public float animationElasticPeriod = 0f;

    [Header("Waypoint animate out")]
    public float outDelay = 0.4f;
    public float animationOutDuration = 0.4f;

}

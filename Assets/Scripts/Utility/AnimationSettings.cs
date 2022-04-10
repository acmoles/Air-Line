using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

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

    [Header("Active indicator")]
    public float activeAlpha = 0.4f;
    public float activeIn = 0.8f;
    public Ease activeInEase = Ease.InOutExpo;
    public float activeOut = 0.8f;
    public Ease activeOutEase = Ease.InOutExpo;

}

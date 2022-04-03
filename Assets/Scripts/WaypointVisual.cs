using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointVisual : MonoBehaviour
{
    public Waypoint waypoint = null;

    [SerializeField]
    private BrushStyles brushStyles = null;

    [SerializeField]
    private AnimationSettings settings = null;

    [SerializeField]
    private SetVertexColors pointColor = null;

    [SerializeField]
    private float scaleModifier = 0.1f;

    private float startedTime = 0;
    private bool hasFinished = false;
    private bool animateForward = true;

    public void SetColor(BrushColor color)
    {
        pointColor.SetColors(brushStyles.GetPrimary(color), brushStyles.GetSecondary(color));
    }

    public void AnimateIn()
    {
        transform.localScale = Vector3.zero;
        startedTime = Time.time;
        hasFinished = false;
        animateForward = true;
    }

    public void AnimateOut()
    {
        startedTime = Time.time;
        hasFinished = false;
        animateForward = false;
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        float currentAnimationTime = Time.time - startedTime - settings.delay;
        float animationProgress = currentAnimationTime / settings.animationDuration;

        if (animateForward)
        {
            float animationValue = settings.inAnimationCurve.Evaluate(animationProgress);
            if (!hasFinished)
            {
                transform.localScale = scaleModifier * Vector3.one * animationValue;
            }
        }
        else
        {
            float animationValue = settings.outAnimationCurve.Evaluate(animationProgress);
            if (!hasFinished)
            {
                transform.localScale = scaleModifier * Vector3.one * -animationValue;
            }
        }



        if (animationProgress > settings.finishedThreshold && !hasFinished)
        {
            hasFinished = true;
        }
    }
}

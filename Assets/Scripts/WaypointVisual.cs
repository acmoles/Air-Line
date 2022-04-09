using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private bool isAnimating = false;
    private bool scheduleAnimate = false;


    public void SetColor(BrushColor color)
    {
        pointColor.SetColors(brushStyles.GetPrimary(color), brushStyles.GetSecondary(color));
    }

    [ContextMenu("Animate in")]
    public void AnimateIn()
    {
        Debug.Log("animate in visual");
        isAnimating = true;
        transform.localScale = Vector3.zero;
        TweenParams tParams = new TweenParams().SetEase(Ease.OutElastic, settings.animationElasticAmp, settings.animationElasticPeriod);
        Tween t = DOTween.To(()=> transform.localScale, x=> transform.localScale = x, Vector3.one * scaleModifier, settings.animationDuration).SetAs(tParams);
        t.OnComplete(() =>
        {
            isAnimating = false;
        });
    }

    [ContextMenu("Animate out")]
    public void AnimateOut()
    {
        if (isAnimating)
        {
            scheduleAnimate = true;
            return;
        }
        transform.localScale = Vector3.one * scaleModifier;
        TweenParams tParams = new TweenParams().SetEase(Ease.InBack);
        DOTween.To(()=> transform.localScale, x=> transform.localScale = x, Vector3.zero, settings.animationOutDuration).SetAs(tParams);
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update() {
        if (scheduleAnimate)
        {
            scheduleAnimate = false;
            AnimateOut();
        }
    }
}

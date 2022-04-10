using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaypointVisual : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    public Waypoint waypoint = null;

    [SerializeField]
    private BrushStyles brushStyles = null;

    [SerializeField]
    private AnimationSettings settings = null;

    [SerializeField]
    private SetVertexColors pointColor = null;

    [SerializeField]
    private float scaleModifier = 0.1f;

    [SerializeField]
    private MeshRenderer activeIndicator = null;
    static MaterialPropertyBlock sharedPropertyBlock;
    private static int colorPropertyId = Shader.PropertyToID("_GlobalAlpha");

    private bool isAnimating = false;
    private bool scheduleAnimate = false;


    public void SetColor(BrushColor color)
    {
        pointColor.SetColors(brushStyles.GetPrimary(color), brushStyles.GetSecondary(color));
    }

    [ContextMenu("Animate in")]
    public void AnimateIn()
    {
        isAnimating = true;
        transform.localScale = Vector3.zero;
        TweenParams tParams = new TweenParams().SetEase(Ease.OutElastic, settings.animationElasticAmp, settings.animationElasticPeriod);
        Tween t = DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.one * scaleModifier, settings.animationDuration).SetAs(tParams);
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
        DOTween.To(() => transform.localScale, x => transform.localScale = x, Vector3.zero, settings.animationOutDuration).SetAs(tParams);
    }

    [ContextMenu("Set next")]
    public void SetNext(int i)
    {
        if(logging) Debug.Log("Set next " + i);
        float currentAlpha = 0f;
        float targetAlpha = settings.activeAlpha;
        TweenParams tParms = new TweenParams().SetEase(settings.activeInEase);
        Tween t = DOTween.To(() => { return currentAlpha; }, x => { currentAlpha = x; }, targetAlpha, settings.activeIn).SetAs(tParms);
        t.OnUpdate(() =>
        {
            SetAlpha(currentAlpha);
        });
    }

    [ContextMenu("Unset next")]
    public void UnsetNext()
    {
        float currentAlpha = 1f;
        const float targetAlpha = 0f;
        TweenParams tParms = new TweenParams().SetEase(settings.activeOutEase);
        Tween t = DOTween.To(() => { return currentAlpha; }, x => { currentAlpha = x; }, targetAlpha, settings.activeOut).SetAs(tParms);
        t.OnUpdate(() =>
        {
            SetAlpha(currentAlpha);
        });
    }

    public void SetAlpha(float currentAlpha)
    {
        if (sharedPropertyBlock == null) {
			sharedPropertyBlock = new MaterialPropertyBlock();
		}
        sharedPropertyBlock.SetFloat(colorPropertyId, currentAlpha);
        activeIndicator.SetPropertyBlock(sharedPropertyBlock);
    }


    private void Start()
    {
        transform.localScale = Vector3.zero;
        SetAlpha(0f);
        
    }

    private void Update()
    {
        if (scheduleAnimate)
        {
            scheduleAnimate = false;
            AnimateOut();
        }
    }
}

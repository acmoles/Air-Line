using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleColorDrawer : MonoBehaviour
{
    [SerializeField]
    private RectTransform colorDrawer = null;

    [SerializeField]
    private CanvasGroup colorDrawerGroup = null;

    [SerializeField]
    private List<Transform> colorButtons = new List<Transform>();

    [SerializeField]
    private float fadeInDuration = 0.8f;

    [SerializeField]
    private float fadeOutDuration = 0.4f;

    private float onAlpha = 1.0f;

    private float offAlpha = 0.0f;

    [SerializeField]
    private bool applyAlpha = true;

    [SerializeField]
    private Vector2 startVectorDisplacement = Vector2.zero;
    [SerializeField]
    private Vector2 originalOpenPosition = Vector2.zero;

    private Tween t = null;
    private float progress = 0f;

    public void ToggleDrawer(string message)
    {
        bool toOpen;

        if (bool.TryParse(message, out toOpen))
        {
            if (t != null) t.Kill();
            float startOpacity = progress;
            float targetOpacity = toOpen ? onAlpha : offAlpha;
            float duration = toOpen ? fadeInDuration : fadeOutDuration;

            Vector2 startPosition = colorDrawer.anchoredPosition;
            Vector2 targetPosition = toOpen ? originalOpenPosition : startVectorDisplacement;

            if (toOpen) colorDrawer.gameObject.SetActive(toOpen);

            t = DOVirtual.Float(startOpacity, targetOpacity, duration, v =>
            {
                progress = v;
                if(applyAlpha) colorDrawerGroup.alpha = v;
                Vector2 vec = Vector2.Lerp(startPosition, targetPosition, Mathf.InverseLerp(startOpacity, targetOpacity, v));
                colorDrawer.anchoredPosition = vec;
            });
            t.OnComplete(() =>
            {
                colorDrawer.gameObject.SetActive(toOpen);
                t = null;
            });
            if (toOpen) t.SetEase(Ease.OutQuart);

            // Scale in bounce buttons
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }
}

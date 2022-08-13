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


    //TODO separate script for sequence drawer
    public void ToggleDrawer(string message)
    {
        bool messageBool;

        if (bool.TryParse(message, out messageBool))
        {
            float startOpacity = messageBool ? 0f : 1f;
            float targetOpacity = messageBool ? 1f : 0f;
            float duration = messageBool ? fadeInDuration : fadeOutDuration;
            if (messageBool) colorDrawer.gameObject.SetActive(messageBool);
            Tween t = DOVirtual.Float(startOpacity, targetOpacity, duration, v => colorDrawerGroup.alpha = v);
            t.OnComplete(() =>
            {
                colorDrawer.gameObject.SetActive(messageBool);
            });

            //transform.DOMove(new Vector3(2,3,4), 1);

            //colorDrawer.SetActive(messageBool);

            // Fade up in box
            // Scale in bounce buttons
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }
}

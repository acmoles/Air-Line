using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorIndicator : MonoBehaviour
{
    [SerializeField]
    private Material material = null;

    [SerializeField]
    private float duration = 0.4f;

    [SerializeField]
    private BrushStyles brushStyles = null;

    public BrushColor overrideColor = BrushColor.Blue;

    [SerializeField]
    private bool useOverrideColor = false;

    private Color currentColor = Color.white;
    private Color currentSecondaryColor = Color.white;
    private Color currentBorderColor = Color.white;

    private void Start()
    {
        currentColor = brushStyles.CustomColor;
        currentSecondaryColor = brushStyles.SecondaryColor;
        currentBorderColor = brushStyles.BorderColor;
    }

    public void OnBrushStylesChanged(string message)
    {
        // Set target color

        Color targetColor;
        Color targetSecondaryColor;
        Color targetBorderColor;

        if (useOverrideColor)
        {
            if (overrideColor == BrushColor.Custom)
            {
                targetColor = Color.white;
                targetSecondaryColor = Color.white;
                targetBorderColor = brushStyles.GetBorder(overrideColor);
            }
            else
            {
                targetColor = brushStyles.GetPrimary(overrideColor);
                targetSecondaryColor = brushStyles.GetSecondary(overrideColor);
                targetBorderColor = brushStyles.GetBorder(overrideColor);
            }
        }
        else
        {
            if (brushStyles.BrushColor == BrushColor.Custom)
            {
                targetColor = Color.white;
                targetSecondaryColor = Color.white;
                targetBorderColor = brushStyles.BorderColor;
            }
            else
            {
                targetColor = brushStyles.CustomColor;
                targetSecondaryColor = brushStyles.SecondaryColor;
                targetBorderColor = brushStyles.BorderColor;
            }
        }

        // Tween to target
        DOTween.To(()=> currentColor, x=> currentColor = x, targetColor, duration).SetEase(Ease.InOutQuint);
        DOTween.To(()=> currentSecondaryColor, x=> currentSecondaryColor = x, targetSecondaryColor, duration).SetEase(Ease.InOutQuint);
        DOTween.To(()=> currentBorderColor, x=> currentBorderColor = x, targetBorderColor, duration).SetEase(Ease.InOutQuint).OnUpdate(UpdateColorUniform);
    }

    private void UpdateColorUniform()
    {
        material.SetColor("_Color", currentColor);
        material.SetColor("_ColorSecondary", currentSecondaryColor);
        material.SetColor("_ColorBackground", currentBorderColor);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ColorIndicator))]
public class ColorIndicatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ColorIndicator t = target as ColorIndicator;
        BrushColor newValue = (BrushColor)EditorGUILayout.EnumPopup( t.overrideColor );
        if( newValue != t.overrideColor)
        {
            t.overrideColor = newValue;
            t.OnBrushStylesChanged("override");
        }
    }
}
#endif


// if (useOverrideColor)
// {
//     if (overrideColor == BrushColor.Custom)
//     {
//         material.SetColor("_Color", Color.white);
//         material.SetColor("_ColorSecondary", Color.white);
//         material.SetColor("_ColorBackground", brushStyles.GetBorder(overrideColor));
//     }
//     else
//     {
//         material.SetColor("_Color", brushStyles.GetPrimary(overrideColor));
//         material.SetColor("_ColorSecondary", brushStyles.GetSecondary(overrideColor));
//         material.SetColor("_ColorBackground", brushStyles.GetBorder(overrideColor));
//     }
// }
// else
// {
//     if (brushStyles.BrushColor == BrushColor.Custom)
//     {
//         material.SetColor("_Color", Color.white);
//         material.SetColor("_ColorSecondary", Color.white);
//         material.SetColor("_ColorBackground", brushStyles.BorderColor);
//     }
//     else
//     {
//         material.SetColor("_Color", brushStyles.CustomColor);
//         material.SetColor("_ColorSecondary", brushStyles.SecondaryColor);
//         material.SetColor("_ColorBackground", brushStyles.BorderColor);
//     }
// }
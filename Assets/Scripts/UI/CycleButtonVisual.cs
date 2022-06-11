using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleButtonVisual : ToggleResponder
{
    [SerializeField]
    private UIManager manager = null;

    [SerializeField]
    private List<GameObject> buttonVisuals = new List<GameObject>();

    //TODO animation
    protected override void SetVisual()
    {
        int index = manager.BrushSizeIndex;
        for (int i = 0; i < buttonVisuals.Count; i++)
        {
            buttonVisuals[i].SetActive(index == i);
        }
    }
}

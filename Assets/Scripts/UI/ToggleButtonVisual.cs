using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable
{
    private bool m_isToggled = false;
    public bool IsToggled
    {
        get => m_isToggled;
        set => m_isToggled = value;
    }
}

public class ToggleButtonVisual : MonoBehaviour
{
    [SerializeField]
    private Interactable toggle = null;

    [SerializeField]
    private GameObject OffVisual = null;

    [SerializeField]
    private GameObject OnVisual = null;

    private void Start()
    {
        SetVisual(toggle.IsToggled);
    }

    private void Update()
    {
        UpdateVisual();
    }

    public void SetVisual(bool isOn)
    {
        OnVisual.SetActive(isOn);
        OffVisual.SetActive(!isOn);
    }

    public void UpdateVisual()
    {
        SetVisual(toggle.IsToggled);
    }
}

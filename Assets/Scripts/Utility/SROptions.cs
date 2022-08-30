using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    [Category("Utilities")]
    public void ClearPlayerPrefs()
    {
        Debug.Log("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }

    [Category("Utilities")]
    public void ToggleUI()
    {
        Debug.Log("Toggling UI");
        UIManager ui = GameObject.FindObjectOfType<UIManager>();
        if (ui == null) { Debug.LogError("UI manager not found!"); return; }
        ui.gameObject.SetActive(!ui.gameObject.activeSelf);
    }

    [Category("Utilities")]
    public void ForceAnchorPlacement()
    {
        Debug.Log("Placing anchor");
        PlaceOnPlane placement = GameObject.FindObjectOfType<PlaceOnPlane>();
        if (placement == null) { Debug.LogError("PlaceOnPlane not found!"); return; }
        placement.SetAllowPlacement(true);
        placement.AddObject(new Vector2(Screen.width / 2, Screen.height / 2), Time.time);
    }

    [Category("Utilities")]
    public void RemovePlacements()
    {
        PlaceOnPlane placement = GameObject.FindObjectOfType<PlaceOnPlane>();
        if (placement == null) { Debug.LogError("PlaceOnPlane not found!"); return; }
        placement.RemovePlacements();
    }
}

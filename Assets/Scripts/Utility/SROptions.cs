using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    private UIManager cachedManager = null;
    private PlaceOnPlane cachedPlacement = null;

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
        if (cachedManager == null) cachedManager = GameObject.FindObjectOfType<UIManager>();
        if (cachedManager == null) { Debug.LogError("UI manager not found!"); return; }
        cachedManager.gameObject.SetActive(!cachedManager.gameObject.activeSelf);
    }

    [Category("Utilities")]
    public void ForceAnchorPlacement()
    {
        Debug.Log("Placing anchor");
        if (cachedPlacement == null) cachedPlacement = GameObject.FindObjectOfType<PlaceOnPlane>();
        if (cachedPlacement == null) { Debug.LogError("PlaceOnPlane not found!"); return; }
        cachedPlacement.SetAllowPlacement(true);
        cachedPlacement.AddObject(new Vector2(Screen.width / 2, Screen.height / 2), Time.time);
    }

    [Category("Utilities")]
    public void RemovePlacements()
    {
        if (cachedPlacement == null) cachedPlacement = GameObject.FindObjectOfType<PlaceOnPlane>();
        if (cachedPlacement == null) { Debug.LogError("PlaceOnPlane not found!"); return; }
        cachedPlacement.RemovePlacements();
    }
}

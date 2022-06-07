#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AutoStartLobby
{
    private const string settingMenuPath = "AirLine/Lobby On Start";
    private const string SettingName = "LobbyOnStart";

    public static bool IsEnabled
    {
        get { return EditorPrefs.GetBool(SettingName, true); }
        set { EditorPrefs.SetBool(SettingName, value); }
    }

    [MenuItem(settingMenuPath)]
    private static void ToggleAction()
    {
        IsEnabled = !IsEnabled;
    }

    [MenuItem(settingMenuPath, true)]
    private static bool ToggleActionValidate()
    {
        Menu.SetChecked(settingMenuPath, IsEnabled);
        return true;
    }
}
#endif

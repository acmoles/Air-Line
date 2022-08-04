using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleColorDrawer : MonoBehaviour
{
    [SerializeField]
    private GameObject colorDrawer = null;

    [SerializeField]
    private List<Transform> colorButtons = new List<Transform>();

    //TODO animated transition
    //TODO separate script for sequence drawer
    public void ToggleDrawer(string message)
    {
        bool messageBool;

        if (bool.TryParse(message, out messageBool))
        {
            colorDrawer.SetActive(messageBool);
            // Fade up in box
            // Scale in bounce buttons
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }
}

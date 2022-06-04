using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorDrawer : MonoBehaviour
{
    [SerializeField]
    private GameObject colorDrawer = null;

    //TODO animated transition
    public void ToggleDrawer(string message)
    {
        bool messageBool;

        if (bool.TryParse(message, out messageBool))
        {
            colorDrawer.SetActive(messageBool);
        }
        else
        {
            Debug.LogWarning("Not a valid stringbool");
        }
    }
}

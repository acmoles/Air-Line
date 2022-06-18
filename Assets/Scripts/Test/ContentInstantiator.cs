using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ContentInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = null;

    [SerializeField]
    private string lobbyName = "Lobby";

    private void Start()
    {
        // in case we started with the wrong scene active, load the lobby scene
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(lobbyName);
            return;
        }
    }
    public void Instantiate()
    {
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(ContentInstantiator))]
public class ContentInstantiatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // get the chosen game object
        ContentInstantiator t = target as ContentInstantiator;

        if (GUILayout.Button("Instantiate"))
        {
            t.Instantiate();
        }

    }
}

#endif

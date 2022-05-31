using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviour
{
    // Start Method
    void Start()
    {
        //1
        PlayerPrefs.DeleteAll();

        Debug.Log("Connecting to Photon Network");

        //2
        //roomJoinUI.SetActive(false);
        //buttonLoadArena.SetActive(false);

        //3
        //ConnectToPhoton();
    }

    void Awake()
    {
        //4 
        PhotonNetwork.AutomaticallySyncScene = true;
    }

}

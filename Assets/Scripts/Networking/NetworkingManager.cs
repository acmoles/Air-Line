using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class NetworkingManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    static public NetworkingManager Instance;

    [SerializeField]
    private bool logging = true;

    [Tooltip("The prefab for representing the photon player")]
    [SerializeField]
    private GameObject networkedPlayerPrefab = null;

    [Header("Local turtle")]
    [Tooltip("The local turtle transform to follow with the local photon player")]
    [SerializeField]
    private Transform localTurtle = null;

    [SerializeField]
    private Transform contentParent = null;


    [SerializeField]
    private string lobbyName = "Lobby";

    [Header("Brush styles")]
    [SerializeField]
    private BrushStyles myBrushStyles = null;
    public const byte brushStylesChangedEventCode = 1;

    private void Start()
    {
        if (!AutoStartLobby.IsEnabled) return;

        Instance = this;

        // in case we started with the wrong scene active, load the lobby scene
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(lobbyName);
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            BrushStylesSync.ShouldManageOwnBrushStyles = true;
            Debug.LogFormat("IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
        else
        {
            BrushStylesSync.ShouldManageOwnBrushStyles = false;
        }

        if (networkedPlayerPrefab == null)
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Networking Manager'", this);
        }
        else
        {
            if (PhotonPlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("Instantiating LocalPlayer in {0}", SceneManagerHelper.ActiveSceneName);

                // Set cached content parent first
                PhotonPlayerManager.contentParentCached = contentParent;

                // Spawn the prefab for the local player. it gets synced by using PhotonNetwork.Instantiate.
                GameObject networkedPlayer = PhotonNetwork.Instantiate(this.networkedPlayerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                networkedPlayer.GetComponent<PhotonPlayerManager>().localTurtleTransform = localTurtle;
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnBrushStylesChanged(string state)
    {
        // TODO individual brushStyles for each client
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (logging) Debug.Log("Sending brush styles networked: " + state);
        // BrushStyles handles it's own serialization
        string brushStylesMessage = myBrushStyles.SerializeBrushStyles();

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(brushStylesChangedEventCode, brushStylesMessage, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        // TODO individual brushStyles for each client
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }
        byte eventCode = photonEvent.Code;
        if (eventCode == brushStylesChangedEventCode)
        {
            string message = (string)photonEvent.CustomData;
            myBrushStyles.DeSerializeBrushStyles(message);
            if (logging) Debug.Log("Incoming brush styles networked: " + message);
        }
    }


    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lobbyName);
    }

    #endregion
}

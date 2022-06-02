using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private Text feedbackText;

    [SerializeField]
    private byte maxPlayersPerRoom = 2;

    [SerializeField]
    private byte minPlayersPerRoom = 1;

    [SerializeField]
    private string arenaToLoad = "ScriptTest";

    public bool isConnecting;

    string gameVersion = "1";

    [Space(10)]
    public InputField playerNameField;
    public InputField roomNameField;

    [Space(5)]
    public Text playerStatus;
    public Text connectionStatus;

    [Space(5)]
    public GameObject roomJoinUI;
    public GameObject buttonLoadArena;
    public GameObject buttonJoinRoom;

    string playerName = "";
    string roomName = "";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("Connecting to Photon Network");

        roomJoinUI.SetActive(false);
        buttonLoadArena.SetActive(false);

        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        isConnecting = true;
        roomJoinUI.SetActive(false);
        // TODO loading
        connectionStatus.text = "Connecting...";
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room " + roomNameField.text);
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
        }
    }

    public void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Player count : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.LogFormat("PhotonNetwork : Loading Arena : {0}", arenaToLoad);
        
        connectionStatus.text = "Loading arena...";
        connectionStatus.color = Color.white;
        if (PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersPerRoom && PhotonNetwork.CurrentRoom.PlayerCount <= maxPlayersPerRoom)
        {
            PhotonNetwork.LoadLevel(arenaToLoad);
        }
        else
        {
            playerStatus.text = "Player requirements to Load Arena not met!";
        }
    }

    // Photon Methods
    public override void OnConnected()
    {
        base.OnConnected();
        isConnecting = false;
        connectionStatus.text = "Connected to Photon!";
        connectionStatus.color = Color.green;
        roomJoinUI.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        controlPanel.SetActive(true);
        Debug.LogError("Disconnected. Please check your Internet connection.");
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonLoadArena.SetActive(true);
            buttonJoinRoom.SetActive(false);
            playerStatus.text = "Your are Lobby Leader";
        }
        else
        {
            playerStatus.text = "Connected to Lobby";
        }
    }



    // Helper Methods
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }
}

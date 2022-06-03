using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class PhotonPlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private bool logging = true;

    [Tooltip("The local photon player instance")]
    public static PhotonPlayerManager LocalPlayerInstance;

    [Tooltip("The list of remote photon players")]
    public static List<PhotonPlayerManager> remoteNetworkedPlayers = new List<PhotonPlayerManager>();

    [HideInInspector]
    public Transform localTurtleTransform = null;

    public PhotonView PlayerPhotonView
    {
        get
        {
            return photonView;
        }
    }

    public string UserId
    {
        get
        {
            return photonView.Owner.UserId;
        }
    }

    public void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this;
        }
        else
        {
            remoteNetworkedPlayers.Add(this);
            if(logging) Debug.Log("Adding networked player: " + remoteNetworkedPlayers.Count);
        }
        DontDestroyOnLoad(gameObject);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public void Update()
    {
        // only if we are the local player
        if (photonView.IsMine)
        {
            transform.localPosition = localTurtleTransform.localPosition;
            transform.localRotation = localTurtleTransform.localRotation;
        }
    }
}

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

    [Tooltip("Cache the ContentParent")]
    public static Transform contentParentCached;

    [Tooltip("The list of remote photon players")]
    public static List<PhotonPlayerManager> remoteNetworkedPlayers = new List<PhotonPlayerManager>();

    [Header("Local turtle")]
    [HideInInspector]
    public Transform localTurtleTransform = null;

    [Header("Fake turtles")]
    [SerializeField]
    private bool enableFakeTurtles = false;
    [Tooltip("Prefab for fake turtles to match remote photon players")]
    [SerializeField]
    private Transform fakeTurtlePrefab = null;
    [Tooltip("The list of fake turtles to update")]
    public static List<ExposeChildTransform> fakeTurtles = new List<ExposeChildTransform>();

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

    public string NickName
    {
        get
        {
            return photonView.Owner.NickName;
        }
    }

    public void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this;
            gameObject.name += "_Local";
        }
        else
        {
            remoteNetworkedPlayers.Add(this);
            if (logging) Debug.Log("Adding networked player: " + remoteNetworkedPlayers.Count);
            if (enableFakeTurtles) AddFakeTurtle(NickName);
            gameObject.name += "_" + NickName;
        }
        transform.parent = contentParentCached;
        foreach (var turtle in fakeTurtles)
        {
            turtle.transform.parent = contentParentCached;
        }
    }

    public void OnNetworkManagerInitialized()
    {
        // Set by NetworkingManager in the scene, but null until content is placed. This callback occurs after placement when contentParentCached should be available
        if (contentParentCached != null)
        {
            Debug.Log("Parenting network player to content callback");
            transform.parent = contentParentCached;
            foreach (var turtle in fakeTurtles)
            {
                turtle.transform.parent = contentParentCached;
            }
        }
        else
        {
            Debug.LogError("Cached parent is still null!");
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (enableFakeTurtles) RemoveFakeTurtle(NickName);
    }

    public void Update()
    {
        // only if we are the local player
        if (photonView.IsMine)
        {
            transform.position = localTurtleTransform.position;
            transform.rotation = localTurtleTransform.rotation;
        }
        else
        {
            // Synchronize fake turtles transforms with photon remote players transforms
            if (remoteNetworkedPlayers.Count > 0 && fakeTurtles.Count > 0)
            {
                for (int i = 0; i < remoteNetworkedPlayers.Count; i++)
                {
                    if (fakeTurtles[i].name != remoteNetworkedPlayers[i].NickName) Debug.LogError("Fake turtle name does not match networked player NickName!");
                    fakeTurtles[i].childTransform.position = remoteNetworkedPlayers[i].transform.position;
                    fakeTurtles[i].childTransform.rotation = remoteNetworkedPlayers[i].transform.rotation;
                }
            }
        }
    }

    private void AddFakeTurtle(string id)
    {
        Transform fakeTurtleDrawer = Instantiate(fakeTurtlePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        fakeTurtleDrawer.name = id;
        ExposeChildTransform fakeTurtle = fakeTurtleDrawer.GetComponent<ExposeChildTransform>();
        fakeTurtle.transform.parent = contentParentCached;
        fakeTurtles.Add(fakeTurtle);
        if (logging) Debug.Log("Adding fake turtle at index: " + (fakeTurtles.Count - 1) + " with id: " + id);
    }

    private void RemoveFakeTurtle(string id)
    {
        for (int i = 0; i < remoteNetworkedPlayers.Count; i++)
        {
            if (remoteNetworkedPlayers[i].NickName == id)
            {
                remoteNetworkedPlayers.RemoveAt(i);
                if (fakeTurtles[i].name != id) Debug.LogError("Fake turtle name does not match networked player NickName!");
                fakeTurtles.RemoveAt(i);
            }
        }
    }
}

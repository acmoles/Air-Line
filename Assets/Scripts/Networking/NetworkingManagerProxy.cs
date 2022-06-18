using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingManagerProxy : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent = null;

    [SerializeField]
    private Transform localTurtle = null;

    void Start()
    {
        NetworkingManager.Instance.contentParent = contentParent;
        NetworkingManager.Instance.localTurtle = localTurtle;
        NetworkingManager.Instance.Initialize();
    }

}

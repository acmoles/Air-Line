using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;

public class UnityEventResolver : UnityEvent<Transform> { }

public class ARCloudAnchorManager : Singleton<ARCloudAnchorManager>
{
    [SerializeField]
    private StringEvent cloudAnchorToHostEvent = null;

    [SerializeField]
    private Camera arCamera = null;

    [SerializeField]
    private float resolveAnchorPassedTimeout = 10.0f;

    [SerializeField]
    private ARAnchorManager arAnchorManager = null;

    private ARAnchor pendingHostAnchor = null;

    private ARCloudAnchor cloudAnchor = null;

    private string anchorToResolve;

    private bool anchorUpdateInProgress = false;

    private bool anchorResolveInProgress = false;

    private float safeToResolvePassed = 0;

    private UnityEventResolver resolver = null;

    private void Awake()
    {
        resolver = new UnityEventResolver();
        resolver.AddListener((t) => PlaceOnPlane.Instance.ReCreatePlacement(t));
    }

    private Pose GetCameraPose()
    {
        return new Pose(arCamera.transform.position, arCamera.transform.rotation);
    }


    public void QueueAnchor(ARAnchor arAnchor)
    {
        pendingHostAnchor = arAnchor;
        HostAnchor();
    }

    public void SetAnchorToResolve(string cloudAnchorID)
    {
        anchorToResolve = cloudAnchorID;
    }

    [ContextMenu("Host")]
    public void HostAnchor()
    {
        if (arAnchorManager != null && arCamera != null)
        {
            FeatureMapQuality quality = arAnchorManager.EstimateFeatureMapQualityForHosting(GetCameraPose());
            Debug.Log("HostAnchor executing, quality: " + quality);
        }
        else
        {
            Debug.Log("Anchor manager and/or arCamera are null");
            return;
        }

        cloudAnchor = arAnchorManager.HostCloudAnchor(pendingHostAnchor, 1);

        if (cloudAnchor == null)
        {
            Debug.LogError("Unable to host cloud anchor");
        }
        else
        {
            anchorUpdateInProgress = true;
        }
    }

    [ContextMenu("Resolve")]
    public void Resolve()
    {
        Debug.Log("Resolve executing for anchor: " + anchorToResolve);

        cloudAnchor = arAnchorManager.ResolveCloudAnchorId(anchorToResolve);

        if (cloudAnchor == null)
        {
            Debug.LogError($"Failed to resolve cloud achor id {cloudAnchor.cloudAnchorId}");
        }
        else
        {
            anchorResolveInProgress = true;
        }
    }

    private void CheckHostingProgress()
    {
        CloudAnchorState cloudAnchorState = cloudAnchor.cloudAnchorState;
        if (cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("Anchor successfully hosted");

            anchorUpdateInProgress = false;

            // keep track of cloud anchors added
            anchorToResolve = cloudAnchor.cloudAnchorId;
            cloudAnchorToHostEvent.Trigger(anchorToResolve);
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.LogError($"Fail to host anchor with state: {cloudAnchorState}");
            anchorUpdateInProgress = false;
        }
    }

    private void CheckResolveProgress()
    {
        CloudAnchorState cloudAnchorState = cloudAnchor.cloudAnchorState;

        Debug.Log($"ResolveCloudAnchor state {cloudAnchorState}");

        if (cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log($"CloudAnchorId: {cloudAnchor.cloudAnchorId} resolved");

            resolver.Invoke(cloudAnchor.transform);

            anchorResolveInProgress = false;
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.LogError($"Fail to resolve Cloud Anchor with state: {cloudAnchorState}");

            anchorResolveInProgress = false;
        }
    }

    void Update()
    {
        // check progress of new anchors created
        if (anchorUpdateInProgress)
        {
            CheckHostingProgress();
            return;
        }

        if (anchorResolveInProgress && safeToResolvePassed <= 0)
        {
            // check every (resolveAnchorPassedTimeout)
            safeToResolvePassed = resolveAnchorPassedTimeout;

            if (!string.IsNullOrEmpty(anchorToResolve))
            {
                Debug.Log($"Resolving AnchorId: {anchorToResolve}");
                CheckResolveProgress();
            }
        }
        else
        {
            safeToResolvePassed -= Time.deltaTime * 1.0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARPointCloudManager))]
public class ARAnchorExperienceManager : MonoBehaviour
{
    [SerializeField]
    private NetworkingSettings settings = null;

    [SerializeField]
    private UnityEvent OnMasterInitialized = null;

    [SerializeField]
    private UnityEvent OnViewerInitialized = null;

    private ARPlaneManager arPlaneManager = null;

    private bool Initialized { get; set; }

    private bool AllowCloudAnchorDelay { get; set; }

    private float timePassedAfterPlanesDetected = 0;

    [SerializeField]
    private float maxScanningAreaTime = 30;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlanesChanged;

#if UNITY_EDITOR
        Activate();
#endif
    }

    void Update()
    {
        if (AllowCloudAnchorDelay)
        {
            if (timePassedAfterPlanesDetected <= maxScanningAreaTime)
            {
                timePassedAfterPlanesDetected += Time.deltaTime * 1.0f;
                Debug.Log($"Experience starts in {maxScanningAreaTime - timePassedAfterPlanesDetected} sec(s)");
            }
            else
            {
                timePassedAfterPlanesDetected = maxScanningAreaTime;
                Activate();
            }
        }
    }

    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!Initialized)
        {
            AllowCloudAnchorDelay = true;
        }
    }

    private void Activate()
    {
        if (settings.isMasterClient)
        {
            Debug.Log("Master places the anchor");
            OnMasterInitialized?.Invoke();
            Initialized = true;
            AllowCloudAnchorDelay = false;
        }
        else
        {
            Debug.Log("Non-master resolves the anchor");
            OnViewerInitialized?.Invoke();
            Initialized = true;
            AllowCloudAnchorDelay = false;
        }
    }
}

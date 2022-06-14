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
    private UnityEvent OnInitialized = null;

    private ARPlaneManager arPlaneManager = null;
    private ARPointCloudManager arPointCloudManager = null;

    private bool Initialized { get; set; }

    private bool AllowCloudAnchorDelay { get; set; }
    
    private float timePassedAfterPlanesDetected = 0;

    [SerializeField]
    private float maxScanningAreaTime = 30;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPointCloudManager = GetComponent<ARPointCloudManager>();

        arPlaneManager.planesChanged += PlanesChanged;

        #if UNITY_EDITOR
            OnInitialized?.Invoke();
            Initialized = true;
            AllowCloudAnchorDelay = false;
            // arPlaneManager.enabled = false;
            // arPointCloudManager.enabled = false;
        #endif
    }

    void Update() 
    {
        if(AllowCloudAnchorDelay)
        {
            if(timePassedAfterPlanesDetected <= maxScanningAreaTime)
            {
                timePassedAfterPlanesDetected += Time.deltaTime * 1.0f;
                Debug.Log($"Experience starts in {maxScanningAreaTime-timePassedAfterPlanesDetected} sec(s)");
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
        if(!Initialized)
        {
            AllowCloudAnchorDelay = true;
        }
    }

    private void Activate()
    {
        Debug.Log("Activate AR Cloud Anchor Experience");
        OnInitialized?.Invoke();
        Initialized = true;
        AllowCloudAnchorDelay = false;
        // arPlaneManager.enabled = false;
        // arPointCloudManager.enabled = false;
    }
}

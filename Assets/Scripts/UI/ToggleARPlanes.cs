using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARPointCloudManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ToggleARPlanes : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    private bool detectPlanes = true;

    private ARPlaneManager m_ARPlaneManager;
    private ARPointCloudManager m_ARPointCloudManager;

    private void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARPointCloudManager = GetComponent<ARPointCloudManager>();
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);

        m_ARPointCloudManager.SetTrackablesActive(value);
    }

    /// <summary>
    /// Toggles plane detection and the visualization of the planes.
    /// </summary>
    public void TogglePlaneDetection()
    {
        detectPlanes = !detectPlanes;
        //m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
        m_ARPlaneManager.requestedDetectionMode = detectPlanes ? PlaneDetectionMode.Horizontal : PlaneDetectionMode.None;

        string planeDetectionMessage = "";
        //if (m_ARPlaneManager.enabled)
        if(detectPlanes)
        {
            planeDetectionMessage = "Disable Plane Detection and Hide Existing";
            SetAllPlanesActive(true);
        }
        else
        {
            planeDetectionMessage = "Enable Plane Detection and Show Existing";
            SetAllPlanesActive(false);
        }

        if (logging) Debug.Log(planeDetectionMessage);
    }
}

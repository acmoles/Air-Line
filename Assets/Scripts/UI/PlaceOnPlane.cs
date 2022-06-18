using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

using Photon.Pun;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARPointCloudManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PlaceOnPlane : Singleton<PlaceOnPlane>
{
    [SerializeField]
    private bool logging = false;

    private bool detectPlanes = true;

    [SerializeField]
    private NetworkingSettings settings = null;

    private bool AllowPlacement { get; set; }

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    private InputManager inputManager = null;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private ARRaycastManager m_RaycastManager;
    private ARAnchorManager m_arAnchorManager;
    private ARPlaneManager m_ARPlaneManager;
    private ARPointCloudManager m_ARPointCloudManager;

    void Awake()
    {
        inputManager = InputManager.Instance;
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_arAnchorManager = GetComponent<ARAnchorManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARPointCloudManager = GetComponent<ARPointCloudManager>();
    }

    private void OnEnable()
    {
        if (settings.isMasterClient)
        {
            inputManager.OnEndTouch += AddObject;
        }
    }

    private void OnDisable()
    {
        if (settings.isMasterClient)
        {
            inputManager.OnEndTouch -= AddObject;
        }
    }

    public void AddObject(Vector2 position, float time)
    {
        if (!AllowPlacement)
        {
            Debug.Log("Placement not allowed yet");
            return;
        }

        if (m_RaycastManager.Raycast(position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            ARAnchor anchor = null;

            if (spawnedObject == null)
            {
                if (logging) Debug.Log("Creating anchor.");

                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                // Make sure the new GameObject has an ARAnchor component
                anchor = spawnedObject.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    anchor = spawnedObject.AddComponent<ARAnchor>();
                }

                //Obsolete method
                //https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Scripts/AnchorCreator.cs
                //anchor = m_arAnchorManager.AddAnchor(new Pose(hitPose.position, hitPose.rotation));
                //spawnedObject.transform.parent = anchor.transform;

                // Queues anchor for hosting
                ARCloudAnchorManager.Instance.QueueAnchor(anchor);



                //Prevent re-placement after first place
                inputManager.OnEndTouch -= AddObject;

                TogglePlaneDetection();
            }
            // else
            // {
            //     spawnedObject.transform.position = hitPose.position;
            // }
        }
    }

    public void SetAllowPlacement(bool isAllowed)
    {
        AllowPlacement = isAllowed;
    }

    public void RemovePlacements()
    {
        Destroy(spawnedObject);
        spawnedObject = null;
        TogglePlaneDetection();
    }

    public void ReCreatePlacement(Transform transform)
    {
        TogglePlaneDetection();
        spawnedObject = Instantiate(placedPrefab, transform.position, transform.rotation);
        //TODO is this what we want?
        spawnedObject.transform.parent = transform;
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
        if (detectPlanes)
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

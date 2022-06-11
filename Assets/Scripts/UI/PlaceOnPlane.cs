using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class PlaceOnPlane : MonoBehaviour
{

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

    ARRaycastManager m_RaycastManager;

    void Awake()
    {
        inputManager = InputManager.Instance;
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    private void OnEnable()
    {
        inputManager.OnEndTouch += AddObject;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= AddObject;
    }

    public void AddObject(Vector2 position, float time)
    {
        if (m_RaycastManager.Raycast(position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                //Prevent re-placement after first place
                inputManager.OnEndTouch -= AddObject;

                //Disable plane and point visualisation
                ToggleARPlanes toggle = GetComponent<ToggleARPlanes>();
                if(toggle != null) toggle.TogglePlaneDetection();
                else Debug.LogWarning("Toggle AR Planes not found");

                //TODO add anchor
            }
            // else
            // {
            //     spawnedObject.transform.position = hitPose.position;
            // }
        }
    }
}

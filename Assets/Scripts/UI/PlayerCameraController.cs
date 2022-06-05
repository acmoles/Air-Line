using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private bool logging = false;

    [SerializeField]
    private float movementSpeed = 2.0f;

    private SimulatedPlayerInputManager playerInput = null;

    public bool isLookToggled = false;

    private CharacterController controller;

    private Vector2 cameraTurn = Vector2.zero;

    [Header("Player controller")]
    [SerializeField]
    private float radius = 0.02f;
    [SerializeField]
    private float height = 0.1f;


    private void Awake()
    {
        playerInput = SimulatedPlayerInputManager.Instance;
    }

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        controller.radius = radius;
        controller.height = height;
    }

    private void OnEnable()
    {
        playerInput.OnToggleLook += LookToggle;
    }

    private void OnDisable()
    {
        playerInput.OnToggleLook -= LookToggle;
    }

    private void Update()
    {
        Move(playerInput.GetMovement());
        Look(playerInput.GetMouseDelta());
    }

    public void Move(Vector2 position)
    {
        if (logging) Debug.Log("Move " + position);
        Vector3 move = new Vector3(position.x, 0, position.y);
        move = transform.TransformVector(move);
        controller.Move(move * Time.deltaTime * movementSpeed);
    }

    public void Look(Vector2 position)
    {
        if (isLookToggled)
        {
            if (logging) Debug.Log("Look " + position);

            cameraTurn.x += position.x;
            cameraTurn.y += position.y;

            transform.localRotation = Quaternion.Euler(-cameraTurn.y, cameraTurn.x, 0);
        }
    }

    public void LookToggle(bool toggle)
    {
        isLookToggled = toggle;
    }
}

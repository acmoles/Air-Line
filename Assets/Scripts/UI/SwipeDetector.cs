using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    // TODO switch to normalised screen height proportion
    [SerializeField]
    private float minimumDistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshhold = 0.9f;
    [SerializeField]
    private GameObject trail = null;

    private InputManager inputManager;

    private float startTime;
    private Vector3 startPosition;
    private float endTime;
    private Vector3 endPosition;

    private Coroutine movementCoroutine = null;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        trail.SetActive(false);
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector3 position, float time)
    {
        // TODO trail only if swipe starts in UI area (bottom of the screen)
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        movementCoroutine = StartCoroutine("trailMovement");
    }

    private IEnumerator trailMovement()
    {
        while(true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void SwipeEnd(Vector3 position, float time)
    {
        endPosition = position;
        endTime = time;
        trail.SetActive(false);
        StopCoroutine(movementCoroutine);
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (
            Vector3.Distance(startPosition, endPosition) >= minimumDistance
            && (endTime - startTime) <= maximumTime
        )
        {
            //Debug.Log("End swipe: " + Vector3.Distance(startPosition, endPosition));
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = (endPosition - startPosition).normalized;
            SwipeDirection(direction);
        }
    }

    private void SwipeDirection(Vector3 direction)
    {
        if (Vector3.Dot(Vector3.up, direction) > directionThreshhold)
        {
            Debug.Log("Swipe up");
        }
    }
}

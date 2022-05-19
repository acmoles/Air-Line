using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDetector : MonoBehaviour
{

    [SerializeField]
    private float startDelay = 0.2f;


    [SerializeField, Range(0f, 10f)]
    private float holdTime = 4f;


    [SerializeField]

    private GameObject indicator = null;


    // [SerializeField]
    // private float zOffset = -0.5f;


    private Coroutine chargeCoroutine = null;
    //private Coroutine coolCoroutine = null;
    private float startTime;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        indicator.SetActive(false);
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += HoldStart;
        inputManager.OnEndTouch += HoldEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= HoldStart;
        inputManager.OnEndTouch -= HoldEnd;
    }

    private void HoldStart(Vector3 position, float time)
    {
        startTime = time;
        chargeCoroutine = StartCoroutine(indicatorChargeUp(startDelay));
    }

    private IEnumerator indicatorChargeUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            // TODO Fill up ring
            Vector2 position = inputManager.PrimaryPosition2D();
            indicator.transform.position = position;
            if (!indicator.activeSelf) indicator.SetActive(true);
            DetectHoldComplete(Time.time);
            yield return null;
        }
    }


    private IEnumerator indicatorCooldown()
    {
        yield return new WaitForSeconds(2f);
        indicator.transform.localScale = Vector3.one * 0.5f;
        indicator.SetActive(false);
    }


    private void HoldEnd(Vector3 position, float time)
    {
        indicator.SetActive(false);
        StopCoroutine(chargeCoroutine);
        // TODO disable follow-me
    }

    private void DetectHoldComplete(float time)
    {
        if (time - startTime > holdTime)
        {
            Debug.Log("Hold complete");
            indicator.transform.localScale = Vector3.one * 0.75f;
            // TODO enable follow-me
            // TODO prevent placement of waypoint
            StopCoroutine(chargeCoroutine);
            StartCoroutine(indicatorCooldown());
        } 
    }
}

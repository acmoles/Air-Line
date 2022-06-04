using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator = null;

    [SerializeField]
    private Image holdRingFill;

    [SerializeField]
    private Image holdRingBackground;

    [SerializeField]
    StringEvent movementStateUpdated;


    [SerializeField, Range(0f, 10f)]
    private float holdDuration = 3f;
    [SerializeField]
    private float holdCancelMultiplier = 3f;

    private bool holdSustain = false;
    private float holdAmount = 0f;

    [SerializeField]
    private float finishedCooldownDuration = .8f;
    private float finishedCooldownAmount = 0f;
    private bool isCoolingDown = false;


    // [SerializeField]
    // private float zOffset = -0.5f;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        indicator.SetActive(false);
    }

    private void OnEnable()
    {
        inputManager.OnHold += HoldStart;
        inputManager.OnEndTouch += HoldEnd;
    }

    private void OnDisable()
    {
        inputManager.OnHold -= HoldStart;
        inputManager.OnEndTouch -= HoldEnd;
    }

    private void HoldStart(bool isHeld)
    {
        if (isHeld)
        {
            holdSustain = true;
            indicator.SetActive(true);
        }
    }

    private void HoldEnd(Vector3 position, float time)
    {
        holdSustain = false;

        //TODO this results in always setting back to play
        if (movementStateUpdated != null) movementStateUpdated.Trigger(TurtleMovementState.ExitFollowMe.ToString());
    }

    private void LateUpdate()
    {
        if (indicator.activeSelf)
        {
            Vector2 position = inputManager.PrimaryPosition2D();
            indicator.transform.position = position;
        }

        if (isCoolingDown)
        {
            finishedCooldownAmount -= Time.deltaTime / finishedCooldownDuration;

            finishedCooldownAmount = Mathf.Clamp01(finishedCooldownAmount);

            holdRingFill.fillAmount = 1f;

            SetImageAlpha(holdRingFill, finishedCooldownAmount);
            SetImageAlpha(holdRingBackground, finishedCooldownAmount);

            if (finishedCooldownAmount == 0f)
            {
                isCoolingDown = false;
                indicator.SetActive(false);
            }
        }
        else
        {
            holdAmount += (Time.deltaTime * (holdSustain ? 1f : -1f * holdCancelMultiplier)) / holdDuration;

            holdAmount = Mathf.Clamp01(holdAmount);

            holdRingFill.fillAmount = holdAmount;

            float alpha = Mathf.InverseLerp(0f, .2f, holdAmount);
            SetImageAlpha(holdRingFill, alpha);
            SetImageAlpha(holdRingBackground, alpha);

            if (holdAmount == 1f)
            {
                finishedCooldownAmount = 1f;
                holdAmount = 0f;
                isCoolingDown = true;
                if (movementStateUpdated != null) movementStateUpdated.Trigger(TurtleMovementState.FollowMe.ToString());
            }
            else if (holdAmount == 0f)
            {
                indicator.SetActive(false);
            }
        }
    }

    private void SetImageAlpha(Image image, float newAlpha)
    {
        Color newCol = image.color;
        newCol.a = newAlpha;
        image.color = newCol;
    }
}

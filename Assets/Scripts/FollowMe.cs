using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO If follow me enabled

[RequireComponent(typeof(Rigidbody))]
public class FollowMe : MonoBehaviour
{
    [SerializeField]
    private MovingTarget movingTarget;

    [SerializeField]
    private MovementSettings movementSettings;

    private Rigidbody turtleRigidbody = null;

    private Vector3 targetPosition = Vector3.zero;

    private Quaternion targetRotation;

    private void Start()
    {
        targetPosition = this.transform.position;
    }

    private void OnEnable()
    {
        CheckReferences();
        turtleRigidbody.isKinematic = false;
    }

    private void OnDisable() {
        turtleRigidbody.isKinematic = true;
    }

    private void CheckReferences()
    {
        if (turtleRigidbody == null)
        {
            turtleRigidbody = this.GetComponent<Rigidbody>();
        }
    }

    private void UpdateTargetPosition()
    {
        // prevent small changes at brush head
        if ((movingTarget.Target.position - targetPosition).sqrMagnitude > movementSettings.offsetMovementThreshhold)
        {
            targetPosition = movingTarget.Target.position;
        }
    }

    private void FixedUpdate()
    {
        UpdateTargetPosition();

        // hover not really working nicely
        //targetPosition.y += Mathf.Sin((Time.time * movementSettings.hoverFrequency) / Mathf.PI) * movementSettings.hoverAmount;

        Vector3 displacement = targetPosition - this.transform.position;
        Vector3 targetVelocity = displacement * movementSettings.speed;
        if (targetVelocity.magnitude > movementSettings.speedLimit)
        {
            targetVelocity = targetVelocity.normalized * movementSettings.speedLimit;
        }

        // movement force
        Vector3 velocityDifference = targetVelocity - turtleRigidbody.velocity;
        turtleRigidbody.AddForce(velocityDifference * movementSettings.force);
    }

    void LateUpdate()
    {
        // target rotation
        Vector3 lookDisplacement = targetPosition - this.transform.position;
        if (lookDisplacement.sqrMagnitude > movementSettings.offsetMovementThreshhold)
        {
            targetRotation = Quaternion.LookRotation(lookDisplacement);
        }

        transform.rotation = Quaternion.Slerp(targetRotation, transform.rotation, Time.deltaTime * movementSettings.rotateSpeed);
    }
}

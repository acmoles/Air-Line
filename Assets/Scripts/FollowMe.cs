using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowMe : MonoBehaviour
{
    [SerializeField]
    private bool useTouch = false;

    [SerializeField]
    private BrushStyles brushStyles = null;

    [SerializeField]
    private MovingTarget movingTarget = null;

    [SerializeField]
    private TurtleSettings movementSettings = null;

    private Rigidbody turtleRigidbody = null;

    [SerializeField]
    private Transform targetRotationTransform = null;

    private Quaternion targetRotation;

    private InputManager inputManager = null;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        CheckReferences();
        turtleRigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        turtleRigidbody.isKinematic = true;
    }

    private void CheckReferences()
    {
        if (turtleRigidbody == null)
        {
            turtleRigidbody = this.GetComponent<Rigidbody>();
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 position = transform.position;
        Vector3 testPosition = transform.position;
        if (useTouch)
        {
            testPosition = TouchUtils.ScreenToWorld(inputManager.PrimaryPosition2D(), brushStyles.followMeScreenOffset);
        }
        else if (movingTarget != null)
        {
            testPosition = movingTarget.Target.position;
        }

        // prevent small changes at brush head
        if ((testPosition - transform.position).sqrMagnitude > movementSettings.offsetMovementThreshhold)
        {
            position = testPosition;
        }
        return position;
    }

    private void FixedUpdate()
    {
        // hover not really working nicely in follow me
        //targetPosition.y += Mathf.Sin((Time.time * movementSettings.hoverFrequency) / Mathf.PI) * movementSettings.hoverAmount;

        // movement
        Vector3 displacement = GetTargetPosition() - this.transform.position;
        Vector3 targetVelocity = displacement * movementSettings.speed;
        if (targetVelocity.magnitude > movementSettings.speedLimit)
        {
            targetVelocity = targetVelocity.normalized * movementSettings.speedLimit;
        }

        // movement force
        Vector3 velocityDifference = targetVelocity - turtleRigidbody.velocity;
        turtleRigidbody.AddForce(velocityDifference * movementSettings.force);

        // rotation force
        targetRotationTransform.rotation = targetRotation;
        targetRotationTransform.position = this.transform.position;
        AddRotationForce(turtleRigidbody, targetRotationTransform, movementSettings.followMeRotationForce, movementSettings.followMeRotationDamp);
    }

    void LateUpdate()
    {
        // target rotation
        float movingAmount = Mathf.InverseLerp(movementSettings.followMeRestingSpeed, movementSettings.followMeMovingSpeed, turtleRigidbody.velocity.magnitude);

        Vector3 lookDisplacement = GetTargetPosition() - this.transform.position;
        Quaternion restRotation = Quaternion.identity;
        if (lookDisplacement.sqrMagnitude > movementSettings.offsetRotationThreshhold)
        {
            restRotation = Quaternion.LookRotation(lookDisplacement);
        }
        else
        {
            // Vector3 cameraDisplacement = Camera.main.transform.position - this.transform.position;
            // restRotation = Quaternion.LookRotation(cameraDisplacement);
            //Vector3 restDirection = this.transform.forward;
            // float d = Vector3.Dot(restDirection, Vector3.up);
            // restDirection -= d * Vector3.up;
            // if (restDirection.sqrMagnitude < 0.00001f) restRotation = this.transform.rotation;
            // else
            // {
            //     restDirection.Normalize();
            //     restRotation = Quaternion.LookRotation(restDirection, Vector3.up);
            // }
            restRotation = this.transform.rotation;
        }

        Quaternion movingRotation = this.transform.rotation;
        if (turtleRigidbody.velocity.magnitude > movementSettings.followMeRotationSpeedThreshold)
        {
            movingRotation = Quaternion.LookRotation(turtleRigidbody.velocity);
        }
        targetRotation = Quaternion.Slerp(restRotation, movingRotation, movingAmount);

        // Vector3 lookDisplacement = GetTargetPosition() - this.transform.position;
        // if (lookDisplacement.sqrMagnitude > movementSettings.offsetRotationThreshhold)
        // {
        //     targetRotation = Quaternion.LookRotation(lookDisplacement);
        // }

        // transform.rotation = Quaternion.Slerp(targetRotation, transform.rotation, Time.deltaTime * movementSettings.rotateSpeed);
        
    }

    public static void AddRotationForce(Rigidbody current, Transform targetTransform, float force, float damp)
    {
        Vector3 targetAngularVelocity = CalculateTorque3D(current, targetTransform) * force;
        Vector3 forceRequired = targetAngularVelocity - (current.angularVelocity * force * damp);
        current.AddTorque(forceRequired);
    }

    public static Vector3 CalculateTorque3D(Rigidbody current, Transform targetTransform)
    {
        Vector3 torque = Vector3.zero;
        torque += CalculateTorqueAxis(current, targetTransform, Vector3.right);
        torque += CalculateTorqueAxis(current, targetTransform, Vector3.up);
        torque += CalculateTorqueAxis(current, targetTransform, Vector3.forward);
        return torque;
    }

    public static Vector3 CalculateTorqueAxis(Rigidbody current, Transform targetTransform, Vector3 localAxis)
    {
        Vector3 x = Vector3.Cross(current.transform.TransformDirection(localAxis).normalized, targetTransform.TransformDirection(localAxis).normalized);

        // to avoid NaN in some cases when x.magnitude = 1
        float theta = Mathf.Asin((Single)x.magnitude);
        if (float.IsNaN(theta))
        {
            theta = Mathf.Asin(1);
        }

        Vector3 w = x.normalized * theta / 0.02f;
        // Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
        Quaternion q = current.transform.rotation * current.inertiaTensorRotation;
        Vector3 torque = q * Vector3.Scale(current.inertiaTensor, Quaternion.Inverse(q) * w);

        return torque;
    }
}

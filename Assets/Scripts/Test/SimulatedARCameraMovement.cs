using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedARCameraMovement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> transformsToMove = new List<Transform>();

    [SerializeField]
    private Vector3 targetPosition1 = Vector3.zero;

    [SerializeField]
    private float targetAngle1 = 0f;

    [SerializeField]
    private Vector3 targetPosition2 = Vector3.zero;

    [SerializeField]
    private float targetAngle2 = 0f;

    [SerializeField]
    private float speed = 1f;

    private float elapsedTime = 0;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float cosineValue = Mathf.Cos(2.0f * Mathf.PI * speed * elapsedTime);
        Vector3 newPosition = targetPosition1 + (targetPosition2 - targetPosition1) * 0.5f * (1 - cosineValue);
        float newAngle = targetAngle1 + (targetAngle2 - targetAngle1) * 0.5f * (1 - cosineValue);
        Quaternion newRotation = Quaternion.AngleAxis(newAngle, Vector3.up);

        foreach (var t in transformsToMove)
        {
            t.position = newPosition;
            t.rotation = newRotation;
        }
    }
}

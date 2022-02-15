using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    //public float moveTime = 5f;
    public bool logging = false;
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;

    public PositionReporter reporter;
    public ColorReporter colorReporter;

    public bool doSpiral = false;

    // Start is called before the first frame update
    void Start()
    {
        reporter = GetComponent<PositionReporter>();
        colorReporter = GetComponent<ColorReporter>();
        //StartCoroutine(MoveOverSeconds(gameObject, endPosition, moveTime));
        StartCoroutine(DoSequence());
    }

    private IEnumerator DoSequence()
    {
        Debug.Log("Sequence Started!");
        reporter.ScheduleStart();
        yield return null;
        yield return Sequences.DoMain(this);
        reporter.ScheduleStop();
        Debug.Log("Sequence Done!");
        // TODO repeat sequence button in UI
    }

    public IEnumerator Td(float distance) {
        yield return Segment(distance, 0, 90f);
    }

    public IEnumerator Rd(float distance) {
        yield return Segment(distance, 90f, 90f);
    }

    public IEnumerator Ld(float distance) {
        yield return Segment(distance, -90f, 90f);
    }

    public IEnumerator Pd(float distance) {
        yield return Segment(distance, 1800f, 90f);
    }

    public IEnumerator Segment(float distance, float roll, float turn) {
        yield return Move(distance);
        yield return Roll(roll);
        yield return Turn(turn);
    }

    public IEnumerator Turn(float angle) {
        yield return Turn(gameObject, "y", angle, rotateSpeed);
    }

    public IEnumerator Dive(float angle) {
        yield return Turn(gameObject, "x", angle, rotateSpeed);
    }

    public IEnumerator Roll(float angle) {
        yield return Turn(gameObject, "z", angle, rotateSpeed);
    }

    public IEnumerator PointAt(Vector3 target) {
        yield return Turn(gameObject, "target", 0, rotateSpeed, target);
    }

    public IEnumerator Turn(GameObject objectToMove, string axis, float angle, float speed, Vector3? target = null)
    {
        if (logging) Debug.Log("start turn, " + axis + ": " + objectToMove.transform.rotation.eulerAngles);
        // Quaternion start = objectToMove.transform.rotation;
        Quaternion end;
        switch (axis)
        {
            case "x":
                end = objectToMove.transform.rotation * Quaternion.Euler(angle, 0f, 0f);
                break;
            case "y":
                end = objectToMove.transform.rotation * Quaternion.Euler(0f, angle, 0f);
                break;
            case "z":
                end = objectToMove.transform.rotation * Quaternion.Euler(0f, 0f, angle);
                break;
            case "target":
                if (target == null)
                {
                    Debug.Log("no target");
                    end = objectToMove.transform.rotation;
                    break;
                }
                Vector3 direction = Vector3.Normalize(target.Value - objectToMove.transform.position);
                end = Quaternion.LookRotation(direction, objectToMove.transform.up);
                break;
            default:
                end = objectToMove.transform.rotation;
                break;
        }

        // TODO use acceleration/deceleration 
        while (objectToMove.transform.rotation != end)
        {
            objectToMove.transform.rotation = Quaternion.RotateTowards(objectToMove.transform.rotation, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if (logging) Debug.Log("end turn, " + axis + ": " + objectToMove.transform.rotation.eulerAngles);
        yield return null;
    }


    public IEnumerator Move(float distance) {
        yield return Move(gameObject, distance, moveSpeed);
    }

    public IEnumerator MoveToTarget(Vector3 target) {
        float distance = (target - gameObject.transform.position).magnitude;
        yield return Move(gameObject, distance, moveSpeed);
    }

    public IEnumerator Move(GameObject objectToMove, float distance, float speed)
    {
        if (logging) Debug.Log("start move");
        Vector3 start = objectToMove.transform.position;
        Vector3 end = objectToMove.transform.position + objectToMove.transform.forward * distance;
        Vector3 linearPosition = objectToMove.transform.position;
        float dist = (end - start).sqrMagnitude;

        while (objectToMove.transform.position != end)
        {
            linearPosition = Vector3.MoveTowards(linearPosition, end, speed * Time.deltaTime);
            float p = 1 - (end - linearPosition).sqrMagnitude / dist;
            p = EasingFunction.EaseInOutQuart(0f, 1f, p);
            objectToMove.transform.position = Vector3.Lerp(start, end, p);
            yield return new WaitForEndOfFrame();
        }
        if (logging) Debug.Log("end move");
        
        yield return null;
    }
    public static float easeInOutQuart(float x) {
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    }

    public IEnumerator SetColor(Color color)
    {
        colorReporter.SetColor(color);
        yield return null;
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        if (logging) Debug.Log("start move");
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
        if (logging) Debug.Log("end move");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (gameObject.transform.forward * 1f), Color.red);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
/////UI events to hook up/////
- Place waypoint

- Toggle brush up/down (end/start line)
- Change radius
- Change colour
- Replay sequence (on new line?)
- Follow moving target
- Change material
- Debug field for max tube points
*/

public class Turtle : MonoBehaviour
{
    public bool logging = false;

    [SerializeField]
    private WaypointManager waypoints;

    [SerializeField]
    private TurtleSettings settings;

    [SerializeField]
    private FollowMe followMe;

    [SerializeField]
    private PositionReporter reporter;

    [SerializeField]
    private BrushStyles brushStyles;

    private bool isMovingScripted = false;
    private bool isMovingWaypoints = false;
    private bool isWaitingToFreeDraw = false;

    private bool brushDown = false;
    public bool BrushDown
    {
        get
        {
            return brushDown;
        }
        set
        {
            brushDown = value;
            if (brushDown)
            {
                reporter.ScheduleStart();
            }
            else
            {
                reporter.ScheduleStop();
            }
        }
    }

    void Start()
    {
        Sequences.PopulateSequenceList();
        DisableFollowMe();
        StartSequence("Initial");
        BrushDown = true;
    }

    public void StartSequence(string commandString)
    {
        DisableFollowMe();
        if (!isMovingWaypoints && !isMovingScripted)
        {
            StartCoroutine(DoSequence(commandString));
        }
    }

    private IEnumerator DoSequence(string commandString)
    {
        yield return null;

        if (commandString != "Initial")
        {
            // Do scripted sequence
            if (logging) Debug.Log("Sequence Started!");
            isMovingScripted = true;

            yield return Sequences.DoSequence(this, commandString);

            if (logging) Debug.Log("Sequence Done!");
            isMovingScripted = false;
        }

        // Do any waiting waypoints
        if (waypoints.WaypointsToPlay())
        {
            yield return DoWaypoints();
        }
        else if (isWaitingToFreeDraw)
        {
            EnableFollowMe();
        }

        yield return null;
    }

    public void OnTriggerWaypoints()
    {
        if (!isMovingWaypoints && !isMovingScripted)
        {
            StartCoroutine(DoWaypoints());
        }
    }

    public void OnToggleFollowMovingTarget(string state)
    {
        if (logging) Debug.Log("State: " + state);
        if (state == FollowMeState.On.ToString())
        {
            // Toggle on
            if (!isMovingWaypoints && !isMovingScripted)
            {
                if (logging) Debug.Log("Enable follow me");
                EnableFollowMe();
            }
            else
            {
                if (logging) Debug.Log("Set waiting for follow me control");
                isWaitingToFreeDraw = true;
            }
        }
        else if (state == FollowMeState.Off.ToString())
        {
            if (logging) Debug.Log("Disable follow me");
            DisableFollowMe();
        }
    }

    private void EnableFollowMe()
    {
        isWaitingToFreeDraw = false;
        followMe.enabled = true;
    }

    private void DisableFollowMe()
    {
        followMe.enabled = false;
    }

    public void OnToggleBrushDown(string state)
    {
        if (logging) Debug.Log("State: " + state);
        if (state == BrushUpDownState.Up.ToString())
        {
            if (logging) Debug.Log("Set brush up");
            BrushDown = false;
        }
        else if (state == BrushUpDownState.Down.ToString())
        {
            if (logging) Debug.Log("Set brush down");
            BrushDown = true;
        }
    }

    private IEnumerator DoWaypoints()
    {
        if (logging) Debug.Log("Waypoints Started!");
        isMovingWaypoints = true;
        yield return null;
        yield return NextWaypoint();
        yield return null;
        if (logging) Debug.Log("Waypoints Done!");
        if (isWaitingToFreeDraw)
        {
            EnableFollowMe();
        }
    }

    private IEnumerator NextWaypoint()
    {
        for (int i = 0; i < waypoints.points.Count; i++)
        {
            if (waypoints.points[i].played)
            {
                continue;
            }
            waypoints.points[i].played = true;
            waypoints.points[i].next = false;
            if (waypoints.points[i].visual != null) waypoints.points[i].visual.AnimateOut();

            if (i != waypoints.points.Count - 1)
            {
                Waypoint nextWaypoint = waypoints.points[i + 1];
                nextWaypoint.next = true;
                if (nextWaypoint.visual != null) nextWaypoint.visual.SetNext(i + 1);
            }

            yield return SetColor(waypoints.points[i].color);
            yield return GotoTarget(waypoints.points[i].position);
            if (i == waypoints.points.Count - 1)
            {
                if (logging) Debug.Log("No more waypoints to play");
                isMovingWaypoints = false;
            }
            else
            {
                if (logging) Debug.Log("Played a waypoint");
                yield return NextWaypoint();
                break;
            }
        }
    }

    public IEnumerator GotoTarget(Vector3 target)
    {
        if (logging) Debug.Log("goto target, " + target);
        yield return PointAt(target);
        yield return MoveToTarget(target);
    }

    public IEnumerator GotoTargetTurnInstant(Vector3 target)
    {
        if (logging) Debug.Log("goto target, " + target);
        yield return PointAtInstant(target);
        yield return MoveToTarget(target);
    }

    public IEnumerator Td(float distance)
    {
        yield return Segment(distance, 0, 90f);
    }

    public IEnumerator Rd(float distance)
    {
        yield return Segment(distance, 90f, 90f);
    }

    public IEnumerator Ld(float distance)
    {
        yield return Segment(distance, -90f, 90f);
    }

    public IEnumerator Pd(float distance)
    {
        yield return Segment(distance, 1800f, 90f);
    }

    public IEnumerator Segment(float distance, float roll, float turn)
    {
        yield return Move(distance);
        yield return Roll(roll);
        yield return Turn(turn);
    }

    public IEnumerator Turn(float angle)
    {
        yield return Turn(gameObject, "y", angle, settings.rotateSpeed);
    }

    public IEnumerator Dive(float angle)
    {
        yield return Turn(gameObject, "x", angle, settings.rotateSpeed);
    }

    public IEnumerator Roll(float angle)
    {
        yield return Turn(gameObject, "z", angle, settings.rotateSpeed);
    }

    public IEnumerator PointAt(Vector3 target)
    {
        yield return Turn(gameObject, "target", 0, settings.rotateSpeed, target);
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

        if (speed == TurtleSettings.instantRotateSpeed)
        {
            yield return RotateInstant(objectToMove, end);
        }
        else
        {
            yield return RotateWithSpeed(objectToMove, end, speed);
        }

        if (logging) Debug.Log("end turn, " + axis + ": " + objectToMove.transform.rotation.eulerAngles);
        yield return null;
    }

    public IEnumerator RotateWithSpeed(GameObject objectToMove, Quaternion end, float speed)
    {
        // TODO use acceleration/deceleration 
        while (objectToMove.transform.rotation != end)
        {
            objectToMove.transform.rotation = Quaternion.RotateTowards(objectToMove.transform.rotation, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator RotateInstant(GameObject objectToMove, Quaternion end)
    {
        objectToMove.transform.rotation = end;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator TurnInstant(float angle)
    {
        yield return Turn(gameObject, "y", angle, TurtleSettings.instantRotateSpeed);
    }

    public IEnumerator DiveInstant(float angle)
    {
        yield return Turn(gameObject, "x", angle, TurtleSettings.instantRotateSpeed);
    }

    public IEnumerator RollInstant(float angle)
    {
        yield return Turn(gameObject, "z", angle, TurtleSettings.instantRotateSpeed);
    }

    public IEnumerator PointAtInstant(Vector3 target)
    {
        yield return Turn(gameObject, "target", 0, TurtleSettings.instantRotateSpeed, target);
    }

    public IEnumerator Move(float distance)
    {
        yield return Move(gameObject, distance, settings.moveSpeed);
    }

    public IEnumerator MoveToTarget(Vector3 target)
    {
        float distance = (target - gameObject.transform.position).magnitude;
        yield return Move(gameObject, distance, settings.moveSpeed);
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

    public IEnumerator SetColor(BrushColor color)
    {
        brushStyles.BrushColor = color;
        yield return null;
    }

    public IEnumerator SetCustomColor(Color color)
    {
        brushStyles.CustomColor = color;
        yield return null;
    }

    public IEnumerator SetSize(BrushSize size)
    {
        brushStyles.BrushSize = size;
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

    void Update()
    {
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (gameObject.transform.forward * 1f), Color.red);
    }

    public IEnumerator QuadraticArc(Vector3 controlPoint, Vector3 endPoint)
    {
        Vector3[] points = Arc.GetArc(transform.position, controlPoint, endPoint, settings.arcSegments);
        for (int i = 0; i < points.Length; i++)
        {
            if (logging) Debug.Log("Arc Point: " + points[i]);
            yield return GotoTargetTurnInstant(points[i]);
        }
        yield return null;
    }
}

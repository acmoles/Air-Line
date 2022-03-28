using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO function to make an arc from start, end and intermediate points
// Sample arc at specific resolution
// Return sampled points

// In script
// Used by calling vector3[] arcPoints = GetArc(vector3 start, vector3 intermediate, vector3 end)
// foreach arc point:
// instant turtle look at point
// turtle goto point

// In free draw
// Point before toggle is arc start waypoint
// Toggle arc point in UI
// Next placed waypoint is an arc intermediate waypointPoint
// (arc intermediate waypoints need to be styled differently)
// After that, next waypoint is an arc end waypoint
// These three points are used in the GetArc function

public static class Arc
{
    public static Vector3[] GetArc(Vector3 start, Vector3 middle, Vector3 end, int segmentCount)
    {
		Vector3[] controlPoints = new Vector3[3];
		controlPoints[0] = start;
		controlPoints[1] = middle;
		controlPoints[2] = end;

        return FlattenCurve(controlPoints, segmentCount);
    }

    // de Casteljau's algorithm
    public static void DrawCurvePoint(Vector3[] points, float t)
    {
        if (points.Length == 1)
        {
            Debug.Log("point at " + points[0]);
        }
        else
        {
            Vector3[] newPoints = new Vector3[points.Length - 1];
            for (int i = 0; i < newPoints.Length; i++)
            {
                newPoints[i] = (1 - t) * points[i] + t * points[i + 1];
            }
            DrawCurvePoint(newPoints, t);
        }
    }

    public static Vector3[] FlattenCurve(Vector3[] controlPoints, int segmentCount)
    {
        float step = 1f / segmentCount;
		Vector3[] curvePoints = new Vector3[segmentCount];
		
		for (int i = 0; i < curvePoints.Length; i++)
		{
			float t = i * step;
			curvePoints[i] = QuadraticBezier(t, controlPoints);
		}

		return curvePoints;
    }

    public static Vector3 QuadraticBezier(float t, Vector3[] controlPoints)
    {
        float t2 = t * t;
        float mt = 1 - t;
        float mt2 = mt * mt;
        return controlPoints[0] * mt2 + 2 * controlPoints[1] * mt * t + controlPoints[2] * t2;
    }

    public static Vector3 CubicBezier(float t, Vector3[] controlPoints)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        float mt = 1 - t;
        float mt2 = mt * mt;
        float mt3 = mt2 * mt;
        return controlPoints[0] * mt3 + 3 * controlPoints[1] * mt2 * t + 3 * controlPoints[2] * mt * t2 + controlPoints[3] * t3;
    }

}

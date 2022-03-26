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

public class Arc
{
	public Vector3[] polyline; // Polyline vertices

	public Vector3[] GetArc(Vector3 start, Vector3 middle, Vector3 end, int samples) {
		return polyline;
	}

}

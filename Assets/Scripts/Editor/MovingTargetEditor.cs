using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( MovingTarget ) )]
public class MovingTargetEditor : Editor
{
    public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        // get the chosen game object
        MovingTarget t = target as MovingTarget;

		if (GUILayout.Button("Place"))
		{
			t.OnPlace();
		}
	}
}
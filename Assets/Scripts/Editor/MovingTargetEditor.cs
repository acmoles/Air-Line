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

        // TODO able to choose the size and color
        if (GUILayout.Button("Size"))
		{
			t.OnSize();
		}

        if (GUILayout.Button("Red"))
		{
			t.OnColor();
		}
	}
}
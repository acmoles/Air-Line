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

		GUILayout.Space(20);

		GUILayout.Label("Color");

        if (GUILayout.Button("Black"))
		{
			t.OnColor(Color.black);
		}

        if (GUILayout.Button("Magenta"))
		{
			t.OnColor(Color.magenta);
		}

        if (GUILayout.Button("Red"))
		{
			t.OnColor(Color.red);
		}

		if (GUILayout.Button("Cyan"))
		{
			t.OnColor(Color.cyan);
		}

		if (GUILayout.Button("Yellow"))
		{
			t.OnColor(Color.yellow);
		}

		if (GUILayout.Button("Green"))
		{
			t.OnColor(Color.green);
		}

		if (GUILayout.Button("Blue"))
		{
			t.OnColor(Color.blue);
		}

		if (GUILayout.Button("White"))
		{
			t.OnColor(Color.white);
		}


		GUILayout.Space(20);

		GUILayout.Label("Size");

        if (GUILayout.Button("Large"))
		{
			t.OnSize(BrushSize.Large);
		}

		if (GUILayout.Button("Medium"))
		{
			t.OnSize(BrushSize.Medium);
		}

		if (GUILayout.Button("Small"))
		{
			t.OnSize(BrushSize.Small);
		}
	}
}
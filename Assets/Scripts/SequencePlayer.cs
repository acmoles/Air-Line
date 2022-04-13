using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SequencePlayer : MonoBehaviour
{
    public Turtle turtle = null;
}

#if UNITY_EDITOR

[CustomEditor( typeof( SequencePlayer ) )]
public class SequencePlayerEditor : Editor
{
    public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        // get the chosen game object
        SequencePlayer t = target as SequencePlayer;

		if (GUILayout.Button("Play sphere"))
		{
			t.turtle.StartSequence("DoSphere");
		}

		GUILayout.Space(20);

        GUILayout.Label("Sequences");
    }
}

#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public class SequencePlayer : MonoBehaviour
{
    public StringEvent sequenceEvent = null;
}

#if UNITY_EDITOR

[CustomEditor(typeof(SequencePlayer))]
public class SequencePlayerEditor : Editor
{
    private void TrySequence(string commandString, SequencePlayer instance)
    {
        if (Sequences.sequenceList.Contains(commandString))
        {
            instance.sequenceEvent.Trigger(commandString);
            // instance.turtle.StartSequence(commandString);
        }
        else
        {
            Debug.LogWarning("Sequence not found: " + commandString);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // get the chosen game object
        SequencePlayer t = target as SequencePlayer;

        GUILayout.Label("Sequences");

        if (GUILayout.Button("Sphere"))
        {
            TrySequence("Sphere", t);
        }

        if (GUILayout.Button("Braid"))
        {
            TrySequence("Braid", t);
        }

        if (GUILayout.Button("Shape"))
        {
            TrySequence("Shape", t);
        }

        if (GUILayout.Button("Triangle"))
        {
            TrySequence("Triangle", t);
        }

        if (GUILayout.Button("Test"))
        {
            TrySequence("Test", t);
        }

        if (GUILayout.Button("Spiral"))
        {
            TrySequence("Spiral", t);
        }


    }
}

#endif
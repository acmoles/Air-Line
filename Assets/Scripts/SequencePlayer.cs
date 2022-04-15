using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public class SequencePlayer : MonoBehaviour
{
    public Turtle turtle = null;
}

#if UNITY_EDITOR

[CustomEditor(typeof(SequencePlayer))]
public class SequencePlayerEditor : Editor
{
    private void TrySequence(string commandString, SequencePlayer instance)
    {
        if (Sequences.sequenceList.Contains(commandString))
        {
            instance.turtle.StartSequence(commandString);
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

        if (GUILayout.Button("DoSphere"))
        {
            TrySequence("DoSphere", t);
        }

        if (GUILayout.Button("DoBraid"))
        {
            TrySequence("DoBraid", t);
        }

        if (GUILayout.Button("DoShape"))
        {
            TrySequence("DoShape", t);
        }

        if (GUILayout.Button("DoTriangle"))
        {
            TrySequence("DoTriangle", t);
        }

        if (GUILayout.Button("DoTest"))
        {
            TrySequence("DoTest", t);
        }

        if (GUILayout.Button("DoSpiral"))
        {
            TrySequence("DoSpiral", t);
        }


    }
}

#endif
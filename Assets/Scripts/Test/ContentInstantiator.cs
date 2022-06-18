using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ContentInstantiator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = null;

    public void Instantiate()
    {
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(ContentInstantiator))]
public class ContentInstantiatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // get the chosen game object
        ContentInstantiator t = target as ContentInstantiator;

        if (GUILayout.Button("Instantiate"))
        {
            t.Instantiate();
        }

    }
}

#endif

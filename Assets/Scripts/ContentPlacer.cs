using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ContentPlacer : MonoBehaviour
{
    public float incrementScaleAmount = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePosition(Vector3 deltaPosition)
    {

    }

    public void UpdateScale(float scaleIncrement)
    {
        transform.localScale = transform.localScale + new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ContentPlacer))]
public class ContentPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // get the chosen game object
        ContentPlacer t = target as ContentPlacer;

        GUILayout.Label("Scale");

        if (GUILayout.Button("Increment scale"))
        {
            t.UpdateScale(t.incrementScaleAmount);
        }

        if (GUILayout.Button("Decrement scale"))
        {
            t.UpdateScale(-t.incrementScaleAmount);
        }


    }
}

#endif

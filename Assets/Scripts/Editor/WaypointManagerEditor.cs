#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


[CustomEditor( typeof( WaypointManager ) )]
public class WaypointManagerEditor : Editor
{
    public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        // get the chosen game object
        WaypointManager t = target as WaypointManager;

		if (GUILayout.Button("Clear"))
		{
			t.points.Clear();
		}

        if (GUILayout.Button("Save"))
		{
            // Make gameobjects and save to points list
            // Save between restarts? See object management
			Debug.Log("noop");
		}
	}
    void OnSceneGUI()
    {
        // get the chosen game object
        WaypointManager t = target as WaypointManager;

        if( t == null )
            return;

        EventType eventType = Event.current.type;
        using (var check = new EditorGUI.ChangeCheckScope())
        {

            if (eventType != EventType.Repaint && eventType != EventType.Layout)
            {
                ProcessEvent(t, Event.current);
            }

            // Don't allow clicking over empty space to deselect the object
            if (eventType == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }

            if (check.changed)
            {
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }

    public void ProcessEvent(WaypointManager t, Event e)
    {
        if (e.type == EventType.MouseDown && e.button == 0 && e.shift)
        {
            var newPointGlobal = MouseUtility.GetMouseWorldPosition(PathSpace.xyz, 10f);
            var newPointLocal = t.transform.InverseTransformPoint(newPointGlobal);
            Undo.RecordObject(this, "Add segment");
            t.AddPoint(newPointLocal, BrushColor.Orange);
        }
    }
}

#endif
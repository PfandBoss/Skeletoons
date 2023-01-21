using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WaypointCreator))]
public class WaypointCreatorEditor : Editor
{
    private bool _isMousePressed;
    private GameObject _lastWaypoint;
    private GameObject _lastSelected;

    public override void OnInspectorGUI()
    {
        WaypointCreator waypointCreator = (WaypointCreator) target;

        if (GUILayout.Button("Add Waypoint"))
        {
            GameObject waypoint = new GameObject("Waypoint " + (waypointCreator.waypoints.Count + 1));
            waypointCreator.waypoints.Add(waypoint);
            EditorUtility.SetDirty(waypointCreator);
        }

        if (GUILayout.Button("Remove Waypoint"))
        {
            if (waypointCreator.waypoints.Count > 0)
            {
                if (_lastWaypoint != null)
                {
                    waypointCreator.waypoints.Remove(_lastWaypoint);
                    DestroyImmediate(_lastWaypoint);
                    _lastWaypoint = null;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        for (int i = 0; i < waypointCreator.waypoints.Count; i++)
        {
            waypointCreator.waypoints[i] = (GameObject) EditorGUILayout.ObjectField("Waypoint " + (i + 1),
                waypointCreator.waypoints[i], typeof(GameObject), true);
        }
    }


    private void OnSceneGUI()
    {
        Event e = Event.current;
        WaypointCreator waypointCreator = (WaypointCreator)target;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            _lastSelected = Selection.activeGameObject;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _lastWaypoint = new GameObject("Waypoint " + (waypointCreator.waypoints.Count + 1));
                _lastWaypoint.transform.position = hit.point;
                waypointCreator.waypoints.Add(_lastWaypoint);
                EditorUtility.SetDirty(waypointCreator);
            }
        }

        for (int i = 0; i < waypointCreator.waypoints.Count; i++)
        {
            if (waypointCreator.waypoints[i] != null)
            {
                Handles.color = Color.red;
                _lastWaypoint = waypointCreator.waypoints[i];
                _lastWaypoint.transform.position = Handles.FreeMoveHandle(_lastWaypoint.transform.position, Quaternion.identity, 0.3f, Vector3.zero, Handles.SphereHandleCap);

                if (i > 0)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(waypointCreator.waypoints[i - 1].transform.position, waypointCreator.waypoints[i].transform.position);
                }
            }
        }
        if (_lastSelected != null) Selection.activeGameObject = _lastSelected;
    }

}

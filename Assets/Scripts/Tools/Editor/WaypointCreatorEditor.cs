using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

[CustomEditor(typeof(WaypointCreator))]
public class WaypointCreatorEditor : Editor
{
    private bool _isMousePressed;
    private bool _isCreating = false;
    private GameObject _lastWaypoint;
    private GameObject _lastSelected;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        
        WaypointCreator waypointCreator = (WaypointCreator) target;

        _isCreating = EditorGUILayout.Toggle("Create Waypoints: ", _isCreating);

        // Only draw Waypoints when Toggle on
        if (!_isCreating) return;
        
        if(waypointCreator.waypoints.waypoints.Count > 0 && waypointCreator.waypoints.waypoints[0] == null ||
           waypointCreator.waypoints.waypoints.Count == 0)
            waypointCreator.LoadData();

        //Remove last added Waypoint
        if (GUILayout.Button("Remove Waypoint"))
        {
            if (waypointCreator.waypoints.waypoints.Count > 0)
            {
                if (_lastWaypoint != null)
                {
                    waypointCreator.waypoints.waypoints.RemoveAt(waypointCreator.waypoints.waypoints.Count-1);
                    DestroyImmediate(_lastWaypoint);
                    _lastWaypoint = null;
                    for (int i = 0; i < waypointCreator.waypoints.waypoints.Count; i++)
                    {
                        waypointCreator.waypoints.waypoints[i].name = "Waypoint " + (i + 1);
                    }
                    EditorUtility.SetDirty(this);
                    waypointCreator.SaveData();
                }
            }
        }

        // Put the created Game Objects in the path list
        for (int i = 0; i < waypointCreator.waypoints.waypoints.Count; i++)
        {
            waypointCreator.waypoints.waypoints[i] = (Transform) EditorGUILayout.ObjectField("Waypoint " + (i + 1),
                waypointCreator.waypoints.waypoints[i], typeof(GameObject), true).GetComponent<Transform>();
        }
    }


    private void OnSceneGUI()
    {
        //When Toggle off, we want to have the normal Scene View
        if (!_isCreating) return;
        
        Event e = Event.current;
        WaypointCreator waypointCreator = (WaypointCreator)target;

        // Check if User presses Mouse Down
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            _lastSelected = Selection.activeGameObject;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            //Get from mouseclick hitted Game Object
            if (Physics.Raycast(ray, out hit))
            {
                // Create Waypoint object and it to the list
                _lastWaypoint = new GameObject("Waypoint " + (waypointCreator.waypoints.waypoints.Count + 1));
                _lastWaypoint.transform.position = hit.point;
                waypointCreator.waypoints.waypoints.Add(_lastWaypoint.transform);
                EditorUtility.SetDirty(waypointCreator);
                waypointCreator.SaveData();
            }
        }

        if(waypointCreator.waypoints.waypoints.Count > 0)
            _lastWaypoint = waypointCreator.waypoints.waypoints[^1].gameObject;

        if (_lastSelected != null) Selection.activeGameObject = _lastSelected;
    }

}

using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class WaypointCreator : MonoBehaviour
{
    public WaypointList waypoints;

    
    private void Awake()
    {
        if(waypoints.waypoints.Count > 0 && waypoints.waypoints[0] == null ||
           waypoints.waypoints.Count == 0)
            LoadData();
    }

    private void Start()
    {
        // if(waypoints.waypoints.Count > 0 && waypoints.waypoints[0] != null)
        //     SaveData();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.waypoints.Count; i++)
        {
            if (waypoints.waypoints[i] != null)
            {
                // Draw Waypoints
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoints.waypoints[i].position, 0.3f);

                //Draw Lines connecting Waypoints
                if (i > 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(waypoints.waypoints[i - 1].position, waypoints.waypoints[i].position);
                }
            }
        }
    }
    
    public void SaveData()
    {
        WaypointData data = new WaypointData();
        data.positions = new List<Vector3>();
        foreach (Transform waypoint in waypoints.waypoints)
        {
            data.positions.Add(waypoint.position);
        }

        string json = JsonUtility.ToJson(data);
        string path = Application.dataPath + $"/Prefabs/Enemies/WaypointLists/JSonData/WaypointData_{waypoints.name}.json";
        File.WriteAllText(path, json);
    }
    
    public void LoadData()
    {
        string path = Application.dataPath + $"/Prefabs/Enemies/WaypointLists/JSonData/WaypointData_{waypoints.name}.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            WaypointData data = JsonUtility.FromJson<WaypointData>(json);

            waypoints.waypoints.Clear();
            foreach (Vector3 position in data.positions)
            {
                GameObject waypoint = new GameObject("Waypoint");
                waypoint.transform.position = position;
                waypoints.waypoints.Add(waypoint.transform);
            }
        }
    }
}
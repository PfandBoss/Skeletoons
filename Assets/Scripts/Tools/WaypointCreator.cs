using System;
using UnityEngine;
using System.Collections.Generic;

public class WaypointCreator : MonoBehaviour
{
    public WaypointList waypoints;

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
}
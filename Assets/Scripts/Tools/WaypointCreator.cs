using UnityEngine;
using System.Collections.Generic;

public class WaypointCreator : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
    

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoints[i].transform.position, 0.3f);

                if (i > 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(waypoints[i - 1].transform.position, waypoints[i].transform.position);
                }
            }
        }
    }
}
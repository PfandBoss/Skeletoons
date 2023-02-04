using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaypointList")]
public class WaypointList : ScriptableObject
{
    public List<Transform> waypoints;
}

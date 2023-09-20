using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private Transform waypointsObject;
    public List<Transform> waypoints;

    void Awake()
    {
        waypointsObject = GameObject.Find("Waypoints").transform;
        for (int i = 0; i < waypointsObject.childCount; i++)
        {
            waypoints.Add(waypointsObject.GetChild(i));
        }
        GameMaster.waypoints = waypoints;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpawner : MonoBehaviour {

    [SerializeField] private Waypoint waypointPrefab;

    private void Start() {
        GetComponent<DungeonBuilder>().OnDungeonBuilt += SpawnWayPoints;
    }

    private void SpawnWayPoints(DungeonData data) {
        foreach (Room room in data.mainRooms) 
            CreateNewWayPoint(room.position);

        foreach (Room room in data.smallRooms)
            CreateNewWayPoint(room.position);

        foreach (Vector2Int corner in data.corridorCorners)
            CreateNewWayPoint(corner);

        OnComplete();
    }

    private void CreateNewWayPoint(Vector2Int position) {
        Instantiate(waypointPrefab, new Vector3(position.x, waypointPrefab.transform.position.y, position.y), waypointPrefab.transform.rotation, transform);
    }

    private void OnComplete() {
        foreach (Waypoint waypoint in Waypoint.AllWayPoints)
            waypoint.FindVisibleNeighbours();
    }
}

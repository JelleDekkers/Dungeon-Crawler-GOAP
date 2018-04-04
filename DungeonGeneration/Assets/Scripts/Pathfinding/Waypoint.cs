using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public static List<Waypoint> AllWayPoints = new List<Waypoint>();

    public List<Waypoint> neighbours;

    [SerializeField] private LayerMask layerMask;

    [HideInInspector] public Waypoint previous;
    [HideInInspector] public float distance;

    private void Awake() {
        AllWayPoints.Add(this);
    }

    private void OnDestroy() {
        AllWayPoints.Remove(this);
    }

    public void FindVisibleNeighbours() {
        foreach (Waypoint waypoint in AllWayPoints) {
            if (waypoint == this)
                continue;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, waypoint.transform.position - transform.position, out hit, float.MaxValue, layerMask)) {
                Waypoint waypointHit = hit.collider.GetComponent<Waypoint>();
                if (waypointHit != null)
                    neighbours.Add(waypointHit);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x);

        if (neighbours == null)
            return;

        foreach(Waypoint neighbour in neighbours) {
            if(neighbour != null)
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
        }
    }
}

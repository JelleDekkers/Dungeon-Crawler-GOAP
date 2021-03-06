﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

	public float walkSpeed = 5.0f;

	private Stack<Vector3> currentPath;
	private Vector3 currentWaypointPosition;
	private float moveTimeTotal;
	private float moveTimeCurrent;

    public Transform target;

    private void Start() {
        NavigateTo(target.transform.position);
    }

    public void NavigateTo(Vector3 destination) {
		currentPath = new Stack<Vector3> ();
		Waypoint currentNode = FindClosestWaypoint(transform.position);
		Waypoint endNode = FindClosestWaypoint(destination);

		if (currentNode == null || endNode == null || currentNode == endNode)
			return;

        SortedList<float, Waypoint> openList = new SortedList<float, Waypoint>();
        List<Waypoint> closedList = new List<Waypoint> ();
		openList.Add(0, currentNode);
		currentNode.previous = null;
		currentNode.distance = 0f;

		while (openList.Count > 0) {
			currentNode = openList.Values[0];
			openList.RemoveAt(0);
			var dist = currentNode.distance;
			closedList.Add(currentNode);
            if (currentNode == endNode) {
                Debug.Log("path found");
                break;
            }

			foreach (var neighbor in currentNode.neighbours) {
				if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
					continue;

				neighbor.previous = currentNode;
				neighbor.distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;
				float distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;
				openList.Add(neighbor.distance + distanceToTarget, neighbor);
			}
		}

        if (currentNode == endNode) {
			while (currentNode.previous != null) {
				currentPath.Push(currentNode.transform.position);
				currentNode = currentNode.previous;
			}
			currentPath.Push(transform.position);
		}
	}

	public void Stop() {
		currentPath = null;
		moveTimeTotal = 0;
		moveTimeCurrent = 0;
	}

	void Update() {
		if (currentPath != null && currentPath.Count > 0) {
			if (moveTimeCurrent < moveTimeTotal) {
				moveTimeCurrent += Time.deltaTime;
				if (moveTimeCurrent > moveTimeTotal)
					moveTimeCurrent = moveTimeTotal;
				transform.position = Vector3.Lerp (currentWaypointPosition, currentPath.Peek (), moveTimeCurrent / moveTimeTotal);
			} else {
				currentWaypointPosition = currentPath.Pop ();
                if (currentPath.Count == 0) {
                    Stop();
                } else {
                    moveTimeCurrent = 0;
                    moveTimeTotal = (currentWaypointPosition - currentPath.Peek()).magnitude / walkSpeed;
                }
			}
		}
	}

	private Waypoint FindClosestWaypoint(Vector3 target) {
		Waypoint closest = null;
		float closestDist = Mathf.Infinity;
		foreach (var waypoint in Waypoint.AllWayPoints) {
			var dist = (waypoint.transform.position - target).magnitude;
			if (dist < closestDist) {
				closest = waypoint;
				closestDist = dist;
			}
		}

		if (closest != null)
			return closest;

		return null;
	}

}

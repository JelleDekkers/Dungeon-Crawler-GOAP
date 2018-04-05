using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

	public static Vector2Int ToInt(this Vector2 position) {
        return new Vector2Int((int)position.x, (int)position.y);
    }

    public static T GetRandom<T>(this T[] array) {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static Agent GetNearest(this Agent[] array, Transform self) {
        Agent tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = self.position;
        foreach (Agent t in array) {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist) {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}

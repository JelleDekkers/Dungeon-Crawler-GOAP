using System;
using UnityEngine;

public static class Common {

    public static float GetDistance(Vector2Int start, Vector2Int end) {
        return (float)Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2));
    }
}

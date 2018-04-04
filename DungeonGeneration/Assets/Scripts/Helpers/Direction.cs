using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction {
    public static readonly Vector2Int[] directions = new Vector2Int[] {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    public static readonly int Up = 270, Right = 0, Down = 90, Left = 180;

    public static readonly int[] rotations = new int[] {
       Up,
       Right,
       Down,
       Left
    };

    public static int GetOppositeIndex(int index) {
        return (index + directions.Length / 2) % directions.Length;
    }

    public static int GetOppositeRotation(int rotation) {
        rotation -= 180;
        if (rotation < 0)
            rotation += 360;
        return rotation;
    }

    public static int GetRotationalIndex(int rotation) {
        int offset = 270 / 90;
        int index = (rotation / 90 - offset) + directions.Length;
        if (index == directions.Length)
            index = 0;
        return index;
    }
}

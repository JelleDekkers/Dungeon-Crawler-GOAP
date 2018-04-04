using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Corridor {

    public Room start, end;
    public List<Vector2Int> tiles;

    public Corridor(Room start, Room end) {
        this.start = start;
        this.end = end;
        tiles = new List<Vector2Int>();
    }
}

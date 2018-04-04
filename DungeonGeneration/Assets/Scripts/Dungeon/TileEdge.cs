using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEdge {

    public Vector2Int one, two;

    public TileEdge(Vector2Int one, Vector2Int two) {
        this.one = one;
        this.two = two;
    }
}

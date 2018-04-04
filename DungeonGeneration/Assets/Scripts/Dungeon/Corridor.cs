using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Corridor {

    public Room start, end;

    public Corridor(Room start, Room end) {
        this.start = start;
        this.end = end;
    }
}

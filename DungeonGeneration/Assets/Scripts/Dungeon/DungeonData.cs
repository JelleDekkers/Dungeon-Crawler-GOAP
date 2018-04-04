using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData {

    public List<Room> mainRooms, smallRooms;
    public Tile[,] dungeon;
    public Vector2Int gridOffset;
    public HashSet<Vector2Int> corridorCorners;

    public DungeonData(List<Room> mainRooms, List<Room> smallRooms, Tile[,] dungeon, Vector2Int gridOffset, HashSet<Vector2Int> corners) {
        this.mainRooms = mainRooms;
        this.smallRooms = smallRooms;
        this.dungeon = dungeon;
        this.gridOffset = gridOffset;
        corridorCorners = corners;
    }
}

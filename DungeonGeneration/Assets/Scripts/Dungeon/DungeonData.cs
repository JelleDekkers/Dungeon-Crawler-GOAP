using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData {

    public List<Room> mainRooms, smallRooms;
    public Tile[,] dungeon;
    public Vector2Int gridOffset;

    public DungeonData(List<Room> mainRooms, List<Room> smallRooms, Tile[,] dungeon, Vector2Int gridOffset) {
        this.mainRooms = mainRooms;
        this.smallRooms = smallRooms;
        this.dungeon = dungeon;
        this.gridOffset = gridOffset;
    }
}

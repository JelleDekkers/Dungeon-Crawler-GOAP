using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Room {

    public RoomType RoomType { get; private set; }
    public int Id { get; private set; }

    public Vector2Int position;
    public int width, height;
    public bool isMainRoom; // for debug
    public List<Corridor> corridors;
    public List<Transform> walls;

    public Vector2Int UpperLeftCornerPosition {
        get {
            return new Vector2Int(position.x - Mathf.CeilToInt((float)width / 2), position.y + Mathf.CeilToInt((float)height / 2));
        }
    }
    public Vector2Int UpperRightCornerPosition {
        get {
            return new Vector2Int(position.x + Mathf.CeilToInt((float)width / 2), position.y + Mathf.CeilToInt((float)height / 2));
        }
    }
    public Vector2Int BottomLeftCornerPosition {
        get {
            return new Vector2Int(position.x - Mathf.CeilToInt((float)width / 2), position.y - Mathf.CeilToInt((float)height / 2));
        }
    }
    public Vector2Int BottomRightCornerPosition {
        get {
            return new Vector2Int(position.x + Mathf.CeilToInt((float)width / 2), position.y - Mathf.CeilToInt((float)height / 2));
        }
    }

    public Room(int id, Vector2Int position, int width, int height) {
        Id = id;
        this.position = position;
        this.width = width;
        this.height = height;
        corridors = new List<Corridor>();
        walls = new List<Transform>();
    }
    
    public void SetRoomType(RoomType type) {
        RoomType = type;
    }

    public void AddWall(GameObject wall) {
        walls.Add(wall.transform);
    }

    public void Draw() {
        if (isMainRoom)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;

        Vector3 pos = new Vector3(position.x, 0.5F, position.y);
        Gizmos.DrawWireCube(pos, new Vector3(width, 1.5f, height));
        Handles.color = Color.red;
        //Handles.Label(new Vector3(position.x, 1, position.y), Id.ToString() + position.ToString() + '\n' + " w " + width + " h " + height);
        if(RoomType != null)
            Handles.Label(new Vector3(position.x, 1, position.y), Id.ToString() + " " + RoomType.ToString());
    }

    public bool ContainsPosition(int x, int y) {
        return (UpperLeftCornerPosition.x <= x && BottomRightCornerPosition.x >= x &&
                UpperLeftCornerPosition.y >= y && BottomRightCornerPosition.y <= y);        
    }

    public void SeperateFromRooms(List<Room> rooms) {
        foreach (Room overlappingRoom in rooms) {
            if (overlappingRoom == this)
                continue;

            if (IsOverLappingWithRoom(overlappingRoom)) {
                int seperationStrength = 1;
                Vector2Int direction = position - overlappingRoom.position;
                direction.x = Mathf.Clamp(direction.x, -1, 1);
                direction.y = Mathf.Clamp(direction.y, -1, 1);
                position = new Vector2Int(position.x + (direction.x * seperationStrength), position.y + (direction.y * seperationStrength));
                overlappingRoom.position = new Vector2Int(overlappingRoom.position.x - (direction.x * seperationStrength), overlappingRoom.position.y - (direction.y * seperationStrength));
            }
        }
    }

    public bool IsOverlappingAnyRoom(List<Room> rooms) {
        foreach (Room room in rooms) {
            if (room == this)
                continue;
            if (IsOverLappingWithRoom(room))
                return true;
        }
        return false;
    }

    public Room GetNearestRoom(List<Room> rooms, out float distance) {
        Room nearest = rooms[0];
        distance = float.MaxValue;

        foreach (Room room in rooms) {
            if (room == this)
                continue;

            bool flag = false;
            foreach (Corridor corridor in corridors) {
                if (corridor.end == room || corridor.start == room) {
                    flag = true;
                    break;
                }
            }
            if (flag)
                continue;

            if (Common.GetDistance(position, room.position) < distance) {
                nearest = room;
                distance = Common.GetDistance(position, room.position);
            }
        }
        return nearest;
    }

    public Room GetNearestRoom(List<Room> rooms) {
        float distance;
        return GetNearestRoom(rooms, out distance);
    }

    public Corridor CreateCorridor(Room room) {
        Corridor corridor = new Corridor(this, room);
        corridors.Add(corridor);
        room.corridors.Add(corridor);
        return corridor;
    }

    public Corridor CreateCorridorToNearestRoom(List<Room> rooms) {
        Room nearest = GetNearestRoom(rooms);
        return CreateCorridor(nearest);
    }

    private bool IsOverLappingWithRoom(Room room, float extraHorizontalOffset = 0f, float extraVerticalOffset = 0f) {
        Vector2 distance = position - room.position;
        return (Mathf.Abs(distance.x) < (width + room.width) * 0.5f + extraHorizontalOffset && 
                Mathf.Abs(distance.y) < (height + room.height) * 0.5f + extraVerticalOffset);
    }
}

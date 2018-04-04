using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour {

    public DungeonAssets assets;
    public Action<DungeonData> OnDungeonBuilt;

    private List<Room> MainRooms { get { return data.mainRooms; } }
    private List<Room> SmallRooms { get { return data.smallRooms; } }
    private Tile[,] Dungeon { get { return data.dungeon; } }
    private Vector2Int GridOffset { get { return data.gridOffset; } }

    private DungeonData data;
    private List<TileEdge> doors;

    private void Start() {
        GetComponent<DungeonGenerator>().OnGeneratingCompleted += BuildDungeonGameObjects;
    }

    private void BuildDungeonGameObjects(DungeonData data) {
        this.data = data;

        SetRoomTypes();
        CreateFloorGameObjects();
        CreateWalls();
        CreatePillars(MainRooms);
        CreateDecor(MainRooms);
        CreateDecor(SmallRooms);

        if (OnDungeonBuilt != null)
            OnDungeonBuilt(data);
    }

    private void SetRoomTypes() {
        MainRooms[0].SetRoomType(assets.startingRoom);
        MainRooms[MainRooms.Count - 1].SetRoomType(assets.goalRoom);

        for (int i = 1; i < MainRooms.Count - 1; i++)
            MainRooms[i].SetRoomType(assets.mainRoomTypes.GetRandom());

        for (int i = 0; i < SmallRooms.Count; i++)
            SmallRooms[i].SetRoomType(assets.smallRoomTypes.GetRandom());
    }

    private void CreateFloorGameObjects() {
        for (int x = 0; x < Dungeon.GetLength(0); x++) {
            for (int y = 0; y < Dungeon.GetLength(1); y++) {
                if (Dungeon[x, y] != Tile.None) {
                    CreateFloorGameObject(x, y);
                }
            }
        }
    }

    private GameObject CreateFloorGameObject(int x, int y) {
        GameObject floor = Instantiate(assets.floors.GetRandom());
        floor.transform.position = new Vector3(x + GridOffset.x, 0, y + GridOffset.y);
        floor.transform.rotation = Quaternion.Euler(0, Direction.rotations.GetRandom(), 0);
        floor.name = "Tile (" + x + "," + y + ") [" + Dungeon[x, y].ToString() + "]";
        floor.transform.parent = transform;
        return floor;
    }

    private void CreateWalls() {
        doors = new List<TileEdge>();

        // main rooms:
        foreach (Room room in MainRooms)
            CreateMainRoomWall(room);

        // small rooms:
        foreach (Room room in SmallRooms)
            CreateSmallRoomWall(room);

        // corridors:
        for (int x = 0; x < Dungeon.GetLength(0); x++) {
            for (int y = 0; y < Dungeon.GetLength(1); y++) {
                Tile tile = Dungeon[x, y];
                if (tile != Tile.Corridor)
                    continue;

                for (int i = 0; i < Direction.directions.Length; i++) {
                    Vector2Int direction = Direction.directions[i];
                    Tile neighbourTile = GetNeighbourTileTypeWithOffset(x + direction.x, y + direction.y);
                    if (neighbourTile != Tile.Corridor && neighbourTile != Tile.SmallRoomFloor) {
                        if (!HasNeighbourDoor(x + GridOffset.x, y + GridOffset.y, Direction.rotations[i])) {
                            CreateWallGameObject(x + GridOffset.x, y + GridOffset.y, Direction.rotations[i]);
                        }
                    }
                }
            }
        }
    }

    // in for loop?
    private void CreateMainRoomWall(Room room) {
        // Horizontal
        for (int x = room.UpperLeftCornerPosition.x + 1; x < room.BottomRightCornerPosition.x; x++) {
            int y = room.UpperLeftCornerPosition.y - 1;
            if (GetNeighbourTileTypeWithOffset(x, y + 1, true) == Tile.Corridor) {
                CreateDoor(x, y, Direction.Up);
            } else {
                GameObject wall = CreateWallGameObject(x, y, Direction.Up);
                room.AddWall(wall);
            }

            y = room.BottomLeftCornerPosition.y + 1;
            if (GetNeighbourTileTypeWithOffset(x, y - 1, true) == Tile.Corridor) { 
                CreateDoor(x, y, Direction.Down);
            } else {
                GameObject wall = CreateWallGameObject(x, y, Direction.Down);
                room.AddWall(wall);
            }
        }

        // Vertical
        for (int y = room.BottomLeftCornerPosition.y + 1; y < room.UpperLeftCornerPosition.y; y++) {
            int x = room.BottomLeftCornerPosition.x + 1;
            if (GetNeighbourTileTypeWithOffset(x - 1, y, true) == Tile.Corridor) {
                CreateDoor(x, y, Direction.Left);
            } else {
                GameObject wall = CreateWallGameObject(x, y, Direction.Left);
                room.AddWall(wall);
            }

            x = room.UpperRightCornerPosition.x - 1;
            if (GetNeighbourTileTypeWithOffset(x + 1, y, true) == Tile.Corridor) {
                CreateDoor(x, y, Direction.Right);
            } else {
                GameObject wall = CreateWallGameObject(x, y, Direction.Right);
                room.AddWall(wall);
            }
        }
    }

    private void CreateSmallRoomWall(Room room) {
        // Horizontal
        for (int y = room.BottomLeftCornerPosition.y + 1; y < room.UpperLeftCornerPosition.y; y++) {
            int x = room.BottomLeftCornerPosition.x + 1;
            Tile neighbourTile = GetNeighbourTileTypeWithOffset(x - 1, y, true);
            if (neighbourTile != Tile.Corridor && neighbourTile != Tile.SmallRoomFloor) {
                GameObject wall = CreateWallGameObject(x, y, Direction.Left);
                room.AddWall(wall);
            }

            x = room.UpperRightCornerPosition.x - 1;
            neighbourTile = GetNeighbourTileTypeWithOffset(x + 1, y, true);
            if (neighbourTile != Tile.Corridor && neighbourTile != Tile.SmallRoomFloor) {
                GameObject wall = CreateWallGameObject(x, y, Direction.Right);
                room.AddWall(wall);
            }
        }

        // Vertical
        for (int x = room.UpperLeftCornerPosition.x + 1; x < room.BottomRightCornerPosition.x; x++) {
            int y = room.UpperLeftCornerPosition.y - 1;
            Tile neighbourTile = GetNeighbourTileTypeWithOffset(x, y + 1, true);
            if (neighbourTile != Tile.Corridor && neighbourTile != Tile.SmallRoomFloor) {
                GameObject wall = CreateWallGameObject(x, y, Direction.Up);
                room.AddWall(wall);
            }

            y = room.BottomLeftCornerPosition.y + 1;
            neighbourTile = GetNeighbourTileTypeWithOffset(x, y - 1, true);
            if (neighbourTile != Tile.Corridor && neighbourTile != Tile.SmallRoomFloor) {
                GameObject wall = CreateWallGameObject(x, y, Direction.Down);
                room.AddWall(wall);
            }
        }
    }

    private Tile GetNeighbourTileTypeWithOffset(int xPosition, int yPosition, bool useOffset = false) {
        Vector2Int offset = Vector2Int.zero;
        if (useOffset) {
            offset.x = GridOffset.x;
            offset.y = GridOffset.y;
        }

        if (!DungeonGenerator.IsInsideGrid(xPosition - offset.x, yPosition - offset.y, Dungeon))
            return Tile.None;
        return Dungeon[xPosition - offset.x, yPosition - offset.y];
    }

    private Door CreateDoor(int x, int y, int rotation) {
        int directionalIndex = Direction.GetRotationalIndex(rotation);
        Vector2Int one = new Vector2Int(x, y);
        Vector2Int two = new Vector2Int(x + Direction.directions[directionalIndex].x, y + Direction.directions[directionalIndex].y);
        doors.Add(new TileEdge(one, two));

        Door door = Instantiate(assets.door);
        door.transform.position = new Vector3(x, 0, y);
        door.name = "Door (" + x + "," + y + ")";
        door.transform.parent = transform;
        door.transform.rotation = Quaternion.Euler(0, rotation, 0);
        return door;
    }

    private GameObject CreateWallGameObject(int x, int y, int rotation) {
        GameObject wall = Instantiate(assets.walls.GetRandom());
        wall.transform.position = new Vector3(x, 0, y);
        wall.name = "Wall (" + (x - GridOffset.x) + "," + (y - GridOffset.y) + ")";
        wall.transform.parent = transform;
        wall.transform.rotation = Quaternion.Euler(0, rotation, 0);
        return wall;
    }

    private void CreatePillars(List<Room> mainRooms) {
        foreach (Room room in MainRooms) {
            int pillarAmountHorizontal = Mathf.FloorToInt(room.width / assets.pillarTileFrequency) + 1;
            int pillarAmountVertical = Mathf.FloorToInt(room.height / assets.pillarTileFrequency) + 1;
            float horizontalStep = room.width / pillarAmountHorizontal;
            float verticalStep = room.height / pillarAmountVertical;

            for (int x = 1; x < pillarAmountHorizontal + 1; x++) {
                for(int y = 1; y < pillarAmountVertical + 1; y++) {
                    Instantiate(assets.pillars.GetRandom(), 
                                new Vector3(room.UpperLeftCornerPosition.x + horizontalStep * x, 0, room.BottomLeftCornerPosition.y + verticalStep * y), 
                                Quaternion.Euler(0, Direction.rotations.GetRandom(), 0), transform);
                }
            }
        }
    }

    private void CreateDecor(List<Room> rooms) {
        foreach (Room room in rooms) {
            if(room.RoomType != null)
                room.RoomType.BuildRoom(transform, room, data);
        }
    }

    private bool HasNeighbourDoor(int x, int y, int rotation) {
        int directionalIndex = Direction.GetRotationalIndex(rotation);
        Vector2Int one = new Vector2Int(x, y);
        Vector2Int two = new Vector2Int(x + Direction.directions[directionalIndex].x, y + Direction.directions[directionalIndex].y);

        foreach (TileEdge container in doors) {
            if ((container.one == one && container.two == two) || (container.one == two && container.two == one))
                return true;
        }

        return false;
    }
}

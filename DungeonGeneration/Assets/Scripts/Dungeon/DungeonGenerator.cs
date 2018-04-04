using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public enum Tile {
    None = 0,
    MainRoomFloor = 1,
    SmallRoomFloor = 2,
    Corridor = 3
}

public class DungeonGenerator : MonoBehaviour {

    public int circleRadius;
    public int roomAmountToGenerate = 100; // random minMax
    public IntMinMax roomWidth, roomHeight;
    public AnimationCurve roomSizeBellCurve;
    public int chosenRoomSurfaceAreaMin = 10;
    public IntMinMax corridorsPerRoom = new IntMinMax(1, 3);
    public AnimationCurve corridorAmountBellCurve;
    public Action<DungeonData> OnGeneratingCompleted;

    private List<Room> allRooms, mainRooms, smallRooms, connectedSmallRooms;
    private float totalWidth, totalHeight;
    private Tile[,] dungeon;
    private Vector2Int gridOffset;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ResetDungeon();
            GenerateDungeon();
        }
    }

    public void GenerateDungeon() {
        CreateRooms();
        SeperateRooms();
        CreateGrid();
        SetRoomTypes();
        AddRoomTilesToGrid(mainRooms, Tile.MainRoomFloor);
        CreateCorridors();
        ConnectAllMainRooms();
        RemoveDisconnectedSmallRooms();
        AddRoomTilesToGrid(connectedSmallRooms, Tile.SmallRoomFloor);

        if(OnGeneratingCompleted != null) {
            DungeonData data = new DungeonData(mainRooms, connectedSmallRooms, dungeon, gridOffset);
            OnGeneratingCompleted(data);
        }
    }

    public void ResetDungeon() {
        allRooms = null;
        mainRooms = null;
        dungeon = null;
        smallRooms = null;
        connectedSmallRooms = null;

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    private void CreateRooms() {
        allRooms = new List<Room>();
        totalWidth = 0;
        totalHeight = 0;

        for (int i = 0; i < roomAmountToGenerate; i++)
            allRooms.Add(SpawnRandomRoomInsideCircle(i));
    }

    private Room SpawnRandomRoomInsideCircle(int id) {
        Vector2 position = Random.insideUnitCircle * circleRadius;
        int width = (int)Mathf.Lerp(roomWidth.min, roomWidth.max, roomSizeBellCurve.Evaluate(Random.value));
        int height = (int)Mathf.Lerp(roomHeight.min, roomHeight.max, roomSizeBellCurve.Evaluate(Random.value));

        if (width % 2 == 0)
            width++;
        if (height % 2 == 0)
            height++;

        totalWidth += width;
        totalHeight += height;

        return new Room(id, position.ToInt(), width, height);
    }

    private void SeperateRooms() {
        bool noMoreOverlap = false;
        int maxLoops = 50;
        int currentLoop = 0;

        while (noMoreOverlap == false) {
            noMoreOverlap = true;

            foreach (Room room in allRooms)
                room.SeperateFromRooms(allRooms);

            foreach (Room room in allRooms) {
                if (room.IsOverlappingAnyRoom(allRooms)) {
                    noMoreOverlap = false;
                    break;
                }
            }
            currentLoop++;
            if (currentLoop >= maxLoops)
                break;
        }
    }

    private void CreateGrid() {
        Room room = allRooms[0];
        Vector2Int lowerLeftCornerPosition = room.BottomLeftCornerPosition;
        Vector2Int upperRightCornerPosition = room.UpperRightCornerPosition;

        for (int i = 1; i < allRooms.Count; i++) {
            room = allRooms[i];
            // horizontal left
            if (room.BottomLeftCornerPosition.x < lowerLeftCornerPosition.x)
                lowerLeftCornerPosition.x = room.BottomLeftCornerPosition.x;
            // horizontal right
            if (room.UpperRightCornerPosition.x > upperRightCornerPosition.x)
                upperRightCornerPosition.x = room.UpperRightCornerPosition.x;
            // vertical left
            if (room.UpperLeftCornerPosition.y > upperRightCornerPosition.y)
                upperRightCornerPosition.y = room.UpperLeftCornerPosition.y;
            // vertical right
            if (room.BottomRightCornerPosition.y < lowerLeftCornerPosition.y)
                lowerLeftCornerPosition.y = room.BottomRightCornerPosition.y;
        }

        int horizontalSize = upperRightCornerPosition.x - lowerLeftCornerPosition.x;
        int verticalSize = upperRightCornerPosition.y - lowerLeftCornerPosition.y;
        gridOffset = lowerLeftCornerPosition;
        dungeon = new Tile[horizontalSize, verticalSize];
    }

    private void SetRoomTypes() {
        mainRooms = new List<Room>();
        smallRooms = new List<Room>();

        foreach (Room room in allRooms) {
            if (room.width * room.height > chosenRoomSurfaceAreaMin) {
                room.isMainRoom = true;
                mainRooms.Add(room);
            } else {
                smallRooms.Add(room);
            }
        }
    }

    private void AddRoomTilesToGrid(List<Room> rooms, Tile tileType) {
        foreach (Room room in rooms) {
            for (int x = 0; x < room.width; x++) {
                for (int y = 0; y < room.height; y++) {
                    Vector2Int index = new Vector2Int(room.BottomLeftCornerPosition.x + x - gridOffset.x + 1, room.BottomLeftCornerPosition.y + y - gridOffset.y + 1);
                    dungeon[index.x, index.y] = tileType;
                }
            }
        }
    }

    private void CreateCorridors() {
        connectedSmallRooms = new List<Room>();

        foreach (Room room in mainRooms) {
            int iterations = (int)Mathf.Lerp(corridorsPerRoom.min, corridorsPerRoom.max, corridorAmountBellCurve.Evaluate(Random.value));
            for (int i = 0; i < iterations; i++) {
                Corridor corridor = room.CreateCorridorToNearestRoom(mainRooms);
                AddCorridorToGrid(corridor);
            }
        }
    }

    private void AddCorridorToGrid(Corridor corrider) {
        Room startRoom = corrider.start;
        Room endRoom = corrider.end;

        // Horizontal:
        Room leftRoom = startRoom.position.x <= endRoom.position.x ? startRoom : endRoom;
        Room rightRoom = leftRoom == startRoom ? endRoom : startRoom;
        int verticalIndex = leftRoom.position.y - gridOffset.y;

        for (int x = leftRoom.position.x - gridOffset.x; x < rightRoom.position.x - gridOffset.x; x++) {
            if (dungeon[x, verticalIndex] == Tile.None) {
                dungeon[x, verticalIndex] = Tile.Corridor;
                corrider.tiles.Add(new Vector2Int(x, verticalIndex));

                Room neighbouringSmallRoom = CheckForPossibleNeighbouringSmallRoom(x + gridOffset.x, verticalIndex + gridOffset.y);
                if (neighbouringSmallRoom != null) {
                    connectedSmallRooms.Add(neighbouringSmallRoom);
                    smallRooms.Remove(neighbouringSmallRoom);
                }
            }
        }

        // Vertical:
        Room bottomRoom = startRoom.position.y <= endRoom.position.y ? startRoom : endRoom;
        Room upperRoom = bottomRoom == startRoom ? endRoom : startRoom;
        int horizontalIndex = rightRoom.position.x - gridOffset.x;

        for (int y = bottomRoom.position.y - gridOffset.y; y < upperRoom.position.y + 1 - gridOffset.y; y++) {
            if (dungeon[horizontalIndex, y] == Tile.None) {
                dungeon[horizontalIndex, y] = Tile.Corridor;
                corrider.tiles.Add(new Vector2Int(horizontalIndex, y));

                Room neighbouringSmallRoom = CheckForPossibleNeighbouringSmallRoom(horizontalIndex + gridOffset.x, y + gridOffset.y);
                if (neighbouringSmallRoom != null) {
                    connectedSmallRooms.Add(neighbouringSmallRoom);
                    smallRooms.Remove(neighbouringSmallRoom);
                }
            }
        }
    }

    private Room CheckForPossibleNeighbouringSmallRoom(int x, int y) {
        foreach (Room room in smallRooms) {
            if (room.ContainsPosition(x, y))
                return room;
        }
        return null;
    }

    private void RemoveDisconnectedSmallRooms() {
        foreach (Room room in smallRooms)
            allRooms.Remove(room);
        smallRooms = null;
    }

    private void ConnectAllMainRooms() {
        Stack<Room> roomStack = new Stack<Room>();
        HashSet<Room> visitedRooms = new HashSet<Room>();

        roomStack.Push(mainRooms[0]);

        while (roomStack.Count > 0) {
            Room room = roomStack.Pop();
            visitedRooms.Add(room);

            foreach (Corridor corridor in room.corridors) {
                if (!visitedRooms.Contains(corridor.end))
                    roomStack.Push(corridor.end);
                if (!visitedRooms.Contains(corridor.start))
                    roomStack.Push(corridor.start);
            }
        }

        if (visitedRooms.Count < mainRooms.Count) {
            List<Room> remainingRooms = mainRooms.Except(visitedRooms).ToList();
            Room startRoom;
            Room targetRoom;
            GetClosestRooms(visitedRooms, remainingRooms, out startRoom, out targetRoom);
            Corridor corridor = startRoom.CreateCorridor(targetRoom);
            AddCorridorToGrid(corridor);
            ConnectAllMainRooms();
        }
    }

    private void GetClosestRooms(HashSet<Room> visitedRooms, List<Room> remainingRooms, out Room startingRoom, out Room targetRoom) {
        float smallestDistance = float.MaxValue;
        Room nearestStartRoom = null;
        Room nearestTargetRoom = null;

        foreach (Room room in visitedRooms) {
            float distance;
            Room target = room.GetNearestRoom(remainingRooms, out distance);
            if (distance < smallestDistance) {
                smallestDistance = distance;
                nearestStartRoom = room;
                nearestTargetRoom = target;
            }
        }
        startingRoom = nearestStartRoom;
        targetRoom = nearestTargetRoom;
    }

    // redundant, not removed because it might be valuable later
    private void GetDisconnectedRooms(Vector2Int startingCoordinate, ref Dictionary<Vector2Int, Room> rooms, ref Dictionary<Vector2Int, Room> roomsFound) {
        Tile[,] tilesRemaining = new Tile[dungeon.GetLength(0), dungeon.GetLength(1)];
        Stack<Vector2Int> neighboursStack = new Stack<Vector2Int>();
        neighboursStack.Push(startingCoordinate);

        for (int x = 0; x < dungeon.GetLength(0); x++) {
            for (int y = 0; y < dungeon.GetLength(1); y++) {
                if (dungeon[x, y] == Tile.MainRoomFloor)
                    tilesRemaining[x, y] = Tile.MainRoomFloor;
            }
        }

        while (neighboursStack.Count > 0) {
            Vector2Int coordinate = neighboursStack.Pop();

            foreach (Vector2Int direction in Direction.directions) {
                Vector2Int neighbourCoordinate = coordinate + direction;

                if (IsInsideGrid(neighbourCoordinate.x, neighbourCoordinate.y, dungeon) && tilesRemaining[neighbourCoordinate.x, neighbourCoordinate.y] == Tile.MainRoomFloor) {
                    neighboursStack.Push(neighbourCoordinate);
                    tilesRemaining[neighbourCoordinate.x, neighbourCoordinate.y] = Tile.None;

                    if (rooms.ContainsKey(neighbourCoordinate + gridOffset)) {
                        roomsFound[neighbourCoordinate + gridOffset] = rooms[neighbourCoordinate + gridOffset];
                        rooms.Remove(neighbourCoordinate + gridOffset);
                    }

                    // for debug
                    GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    visual.transform.SetParent(transform);
                    visual.transform.position = new Vector3(neighbourCoordinate.x + gridOffset.x - 0.5f, 1, neighbourCoordinate.y + gridOffset.y - 0.5f);
                    visual.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    visual.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    public static bool IsInsideGrid(int x, int y, Tile[,] dungeon) {
        return (x >= 0 && x < dungeon.GetLength(0) && y >= 0 && y < dungeon.GetLength(1));
    }

    private void OnGUI() {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Generate"))
            CreateRooms();
        if (GUILayout.Button("Seperate"))
            SeperateRooms();
        if (GUILayout.Button("Create Grid"))
            CreateGrid();
        if (GUILayout.Button("Assign Main Rooms"))
            SetRoomTypes();
        if (GUILayout.Button("Assign main room tiles"))
            AddRoomTilesToGrid(mainRooms, Tile.MainRoomFloor);
        if (GUILayout.Button("Create Corridors"))
            CreateCorridors();
        if (GUILayout.Button("Connect Main Rooms"))
            ConnectAllMainRooms();
        if (GUILayout.Button("Remove disconnected sRooms"))
            RemoveDisconnectedSmallRooms();
        if (GUILayout.Button("Assign small room tiles"))
            AddRoomTilesToGrid(connectedSmallRooms, Tile.SmallRoomFloor);
        if(GUILayout.Button("On Complete")) {
            DungeonData data = new DungeonData(mainRooms, connectedSmallRooms, dungeon, gridOffset);
            OnGeneratingCompleted(data);
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Reset"))
            ResetDungeon();
        GUILayout.EndVertical();
    }

    private void OnDrawGizmos() {
        if (allRooms == null) 
            return;

        foreach (Room r in allRooms)
            r.Draw();

        if (dungeon != null) {
            for (int x = 0; x < dungeon.GetLength(0); x++) { 
                for (int y = 0; y < dungeon.GetLength(1); y++) {
                    Gizmos.color = Color.gray;
                    //Gizmos.DrawWireCube(new Vector3(x + gridOffset.x - 0.5f, 0, y + gridOffset.y - 0.5f), new Vector3(1, 0, 1));
                }
            }
        } else {
            UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.up, circleRadius);

        }
    }
}

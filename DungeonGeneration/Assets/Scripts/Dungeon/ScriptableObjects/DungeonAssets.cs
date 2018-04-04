using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon Assets", menuName = "Dungeon/Assets", order = 0)]
public class DungeonAssets : ScriptableObject {

    public GameObject[] walls;
    public GameObject[] floors;
    public GameObject[] pillars;
    public float pillarTileFrequency = 2;
    public Door door;
    public StartingRoom startingRoom;
    public GoalRoom goalRoom;
    public RoomType[] mainRoomTypes, smallRoomTypes;
}

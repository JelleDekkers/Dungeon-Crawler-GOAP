using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Starting Room", menuName = "Dungeon/Rooms/Starting Room", order = 0)]
public class StartingRoom : RoomType {

    public GameObject[] propsWall;
    public float propPerWallFrequency = 0.5f;

    public override void BuildRoom(Transform dungeon, Room room, DungeonData data) {
        CreateWallProps(room);
    }

    private void CreateWallProps(Room room) {
        foreach(Transform wall in room.walls) {
            if(Random.value <= propPerWallFrequency) {
                Instantiate(propsWall.GetRandom(), wall.position, wall.transform.rotation, wall);
            }
        }
    }
}

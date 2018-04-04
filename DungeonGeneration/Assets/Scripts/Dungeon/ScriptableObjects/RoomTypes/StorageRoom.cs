using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage Room", menuName = "Dungeon/Rooms/Storage Room", order = 2)]
public class StorageRoom : RoomType {

    public GameObject[] propsWall;
    public GameObject[] propsFloor;

    public float propPerWallFrequency = 0.5f;
    public float propPerFloorFrequency = 0.5f;


    public override void BuildRoom(Transform dungeon, Room room, DungeonData data) {
        CreateWallProps(room);
        CreateFloorProps(room, dungeon);
    }

    private void CreateWallProps(Room room) {
        foreach (Transform wall in room.walls) {
            if (Random.value <= propPerWallFrequency) {
                Instantiate(propsWall.GetRandom(), wall.position, wall.transform.rotation, wall);
            }
        }
    }

    private void CreateFloorProps(Room room, Transform transform) {
        int width = room.UpperRightCornerPosition.x - room.UpperLeftCornerPosition.x;
        int height = room.UpperRightCornerPosition.y - room.BottomLeftCornerPosition.y;

        for (int i = 0; i < width * height; i++) {
            if (Random.value <= propPerFloorFrequency) {
                Vector3 rndPosition = new Vector3(room.position.x + Random.Range(-width / 2 + 1, width / 2 - 1), 0, room.position.y + Random.Range(-height / 2 + 1, height / 2 - 1));
                Instantiate(propsFloor.GetRandom(), rndPosition, Quaternion.Euler(0,Random.Range(0, 360), 0), transform);
            }
        }
    }
}

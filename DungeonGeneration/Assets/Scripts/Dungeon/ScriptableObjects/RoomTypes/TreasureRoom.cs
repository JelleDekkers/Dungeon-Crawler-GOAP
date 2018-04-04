using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Treasure Room", menuName = "Dungeon/Rooms/Treasure Room", order = 3)]
public class TreasureRoom : RoomType {

    public GameObject treasure;
    public GameObject carpet;

    public override void BuildRoom(Transform dungeon, Room room, DungeonData data) {
        Quaternion rndRotation = Quaternion.Euler(0, Direction.rotations.GetRandom(), 0);
        Vector3 position = new Vector3(room.position.x, 0, room.position.y);
        Instantiate(treasure, position, rndRotation, dungeon);
        Instantiate(carpet, position, rndRotation, dungeon);
    }
}

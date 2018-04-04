using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goal Room", menuName = "Dungeon/Rooms/Goal Room", order = 1)]
public class GoalRoom : RoomType {

    public DungeonGoal goalObject;
    public GameObject[] propsWall;
    public float propPerWallFrequency = 0.5f;

    public override void BuildRoom(Transform dungeon, Room room, DungeonData data) {
        CreateGoalObject(room, dungeon);
        CreateWallProps(room);
    }

    private void CreateGoalObject(Room room, Transform parent) {
        Instantiate(goalObject, new Vector3(room.position.x, 0, room.position.y), goalObject.transform.rotation, parent);
    }

    private void CreateWallProps(Room room) {
        foreach (Transform wall in room.walls) {
            if (Random.value <= propPerWallFrequency) {
                Instantiate(propsWall.GetRandom(), wall.position, wall.transform.rotation, wall);
            }
        }
    }
}
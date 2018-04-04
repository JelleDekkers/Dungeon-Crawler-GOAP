using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomType : ScriptableObject {

    public new string name;

    public abstract void BuildRoom(Transform dungeon, Room room, DungeonData data);
}

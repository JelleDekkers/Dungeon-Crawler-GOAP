using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnFrequencyPerRoom = 0.7f;
    public IntMinMax enemiesPerRoom;
    public Agent[] enemies;

    private void Start() {
        GetComponent<DungeonBuilder>().OnDungeonBuilt += SpawnEnemies;
    }

    private void SpawnEnemies(DungeonData data) {
        for(int i = 1; i < data.mainRooms.Count; i++) {
            if (Random.value < spawnFrequencyPerRoom)
                InstantiateEnemies(data.mainRooms[i], transform);
        }
    }

    private void InstantiateEnemies(Room room, Transform transform) {
        int width = room.UpperRightCornerPosition.x - room.UpperLeftCornerPosition.x;
        int height = room.UpperRightCornerPosition.y - room.BottomLeftCornerPosition.y;

        int amount = Random.Range(enemiesPerRoom.min, enemiesPerRoom.max);
        for (int i = 0; i < amount; i++) {
            if (Random.value <= spawnFrequencyPerRoom) {
                Vector3 rndPosition = new Vector3(room.position.x + Random.Range(-width / 2 + 1, width / 2 - 1), 0, room.position.y + Random.Range(-height / 2 + 1, height / 2 - 1));
                Instantiate(enemies.GetRandom(), rndPosition, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
            }
        }
    }
}

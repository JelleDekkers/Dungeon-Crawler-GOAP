using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    public DungeonBuilder builder;
    public GameObject player;
    public GameObject playerInstance;
    
    private void Awake() {
        builder.OnDungeonBuilt += SpawnPlayer;
    }

    private void SpawnPlayer(DungeonData data) {
        if (playerInstance != null)
            Destroy(playerInstance);

        Vector3 spawnPos = new Vector3(data.mainRooms[0].position.x, 0, data.mainRooms[0].position.y);
        playerInstance = Instantiate(player, spawnPos, player.transform.rotation);
    }
}

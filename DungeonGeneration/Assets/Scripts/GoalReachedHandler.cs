using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReachedHandler : MonoBehaviour {

    public DungeonGenerator generator;

	private void Start () {
        DungeonGoal.OnPlayerHitGoal += GenerateNewDungeon;
	}
	
    private void GenerateNewDungeon() {
        generator.ResetDungeon();
        generator.GenerateDungeon();
        MovePlayerToStart();
    }

    private void MovePlayerToStart() {

    }

    private void OnDestroy() {
        DungeonGoal.OnPlayerHitGoal -= GenerateNewDungeon;
    }
}

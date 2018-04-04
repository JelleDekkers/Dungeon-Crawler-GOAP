using System;
using UnityEngine;

public class DungeonGoal : MonoBehaviour {

    public static Action OnPlayerHitGoal;

    public void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Player>()) {
            Debug.Log(other.gameObject.name);
            if(OnPlayerHitGoal != null)
                OnPlayerHitGoal();    
        }
    }
}

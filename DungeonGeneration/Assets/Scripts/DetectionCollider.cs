using System;
using UnityEngine;

public class DetectionCollider : MonoBehaviour {

    public Action<Player> OnPlayerSpotted;

    private void OnTriggerEnter(Collider other) {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null) {
            if (OnPlayerSpotted != null)
                OnPlayerSpotted(player);
        }
    }
}

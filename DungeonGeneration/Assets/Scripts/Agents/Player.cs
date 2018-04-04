using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent {

    protected override void Die() {
        base.Die();
        Debug.Log("Game Over");
    }
}

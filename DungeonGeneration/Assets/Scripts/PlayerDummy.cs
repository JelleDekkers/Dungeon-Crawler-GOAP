using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour {

    public float healthPoints = 100f;

    private void Update() {
        if (healthPoints <= 0)
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour {

    [SerializeField] private Animator animator;

    private readonly string blockParameter = "Block";

    private Collider shieldCollider;

    private void Start() {
        shieldCollider = GetComponent<Collider>();
    }

    private void Update() {
        shieldCollider.enabled = animator.GetBool(blockParameter);
    }
}

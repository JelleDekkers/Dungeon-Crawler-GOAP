using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour {

    private Rigidbody[] limbs;
    private Collider hitBoxCollider;

	void Start () {
        limbs = GetComponentsInChildren<Rigidbody>();
        hitBoxCollider = GetComponent<Collider>();
	}

    public void ReleaseLimbs(bool value) {
        foreach (Rigidbody limb in limbs) {
            limb.isKinematic = !value;
            limb.useGravity = value;
        }

        if (hitBoxCollider != null)
            hitBoxCollider.enabled = !value;
    }
}

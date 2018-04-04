using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCharacterController : MonoBehaviour {

    public float acceleration;
    public float friction;
    public float jumpForce;
    public float rotationSpeed = 10;
    public float isGroundedRayLength = 1;

    private Vector3 movementInput = Vector3.zero;
    private Vector3 velocity;
    private Vector3 position;
    private new Rigidbody rigidbody;
    private float rotation = 0;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        Movement();
    }

    public void Jump() {
        if (IsGrounded())
            velocity.y += jumpForce;
    }

    public void SetMovementInput(float x, float z) {
        movementInput.x = x;
        movementInput.z = z;
    }

    public void Rotate(float input) {
        rotation += input * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void LookAt(Vector3 target) {
        var lookPos = target - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
    }

    private void Movement() {
        velocity += movementInput * acceleration * Time.deltaTime;
        transform.position += transform.forward * velocity.z * Time.deltaTime;
        transform.position += transform.right * velocity.x * Time.deltaTime;
        transform.position += transform.up * velocity.y * Time.deltaTime;
        velocity -= friction * Time.deltaTime * velocity;
    }

    private void Rotation() {
    }

    private bool IsGrounded() {
        RaycastHit hit;
        return (Physics.Raycast(transform.position, Vector3.down, out hit, isGroundedRayLength));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up / 10, transform.position + Vector3.down * isGroundedRayLength);
    }
}

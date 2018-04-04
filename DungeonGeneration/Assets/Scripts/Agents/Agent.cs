using System;
using UnityEngine;

public class Agent : MonoBehaviour {

    public float startingHealth = 100;
    public float HealthPoints {
        get {
            return healthPoints;
        }
        private set {
            healthPoints = value;
        }
    }
    public Action OnDeath;

    [SerializeField] private float healthPoints;
    [SerializeField] private float damageAmount = 10;

    private PhysicsCharacterController physics;
    private CustomCharacterController controller;
    private Animator animator;
    private RagdollHandler ragdollHandler;
    private new Rigidbody rigidbody;
    private new Collider collider;

    private void Start() {
        HealthPoints = startingHealth;
        controller = GetComponent<CustomCharacterController>();
        animator = GetComponent<Animator>();
        ragdollHandler = GetComponent<RagdollHandler>();
        controller.OnAgentHit += AttackAgent;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        physics = GetComponent<PhysicsCharacterController>();
    }

    private void AttackAgent(Agent agent) {
        agent.Damage(damageAmount);
    }

    public void Damage(float amount) {
        HealthPoints -= amount;

        if (HealthPoints <= 0)
            Die();
    }

    protected virtual void Die() {
        controller.enabled = false;
        animator.enabled = false;
        ragdollHandler.ReleaseLimbs(true);
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        collider.enabled = false;
        physics.enabled = false;

        if (OnDeath != null)
            OnDeath();
    }
}

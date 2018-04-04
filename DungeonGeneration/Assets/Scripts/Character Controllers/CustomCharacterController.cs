using System;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour {

    public Animator animator;
    public float attackRaycastLength = 1;

    [SerializeField] protected PhysicsCharacterController movementController;
    [SerializeField] protected LayerMask attackRaycastLayerMask;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackForce;

    public Action<Agent> OnAgentHit;

    protected readonly string movementParameter = "Movement";
    protected readonly string attackParameter = "Attack";
    protected readonly string blockParameter = "Block";
    protected readonly string attackState = "Attack";

    public virtual void Block(bool block) {
        animator.SetBool(blockParameter, block);
    }

    public virtual void Jump() {
        movementController.Jump();
    }

    public virtual void Attack() {
        animator.SetTrigger(attackParameter);
    }

    private void AttackTrigger() {
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, transform.forward, out hit, attackRaycastLength, attackRaycastLayerMask)) {
            GameObject objectHit = hit.collider.gameObject;
            Debug.Log(objectHit);

            if (objectHit.GetComponent<Rigidbody>())
                objectHit.GetComponent<Rigidbody>().AddForce(transform.forward * attackForce);

            Agent agentHit = objectHit.GetComponent<Agent>();
            if (agentHit != null)
                OnAgentHit(agentHit);
        }
    }

    private void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + transform.forward * attackRaycastLength);
    }
}

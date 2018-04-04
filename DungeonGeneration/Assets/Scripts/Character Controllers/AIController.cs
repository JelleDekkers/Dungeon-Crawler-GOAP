using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CustomCharacterController {

    public Vector3 Destination { get; private set; }

    public float stoppingDistance = 1;
    public bool move;

    [SerializeField] private float movementAnimationSpeedModifier = 2;

    public void Update() {
        if (!move)
            return;
        
        if(Vector3.Distance(Destination, transform.position) < stoppingDistance) {
            StopMoving();
            return;
        }

        animator.SetFloat(movementParameter, 1 * movementAnimationSpeedModifier);
        movementController.SetMovementInput(0, 1);

        //movementController.Rotate(Input.GetAxis("Mouse X"));
        movementController.LookAt(Destination);
    }

    public override void Jump() {
        base.Jump();
        animator.SetBool(blockParameter, Input.GetMouseButton(1));
    }

    public void SetDestination(Vector3 destination) {
        move = true;
        Destination = destination;
    }

    public void StopMoving() {
        animator.SetFloat(movementParameter, 0);
        movementController.SetMovementInput(0, 0);

        move = false;
    }

    public void ContinueMoving() {
        move = true;
    }
}

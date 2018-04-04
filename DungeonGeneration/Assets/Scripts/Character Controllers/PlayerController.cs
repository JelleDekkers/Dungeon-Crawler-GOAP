using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CustomCharacterController {

    [SerializeField] private float movementAnimationSpeedModifier = 2;

    public void Update() {
        animator.SetFloat(movementParameter, Input.GetAxis("Vertical") * movementAnimationSpeedModifier);

        movementController.SetMovementInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementController.Rotate(Input.GetAxis("Mouse X"));

        animator.SetBool(blockParameter, Input.GetMouseButton(1));

        if (Input.GetMouseButtonDown(0))
            Attack();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
}

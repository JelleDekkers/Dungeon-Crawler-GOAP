using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class PickupWeaponAction : Action {

        public float duration = 1;

        private Weapon targetWeapon;
        private float timer;

        private void Start() {
            GetComponent<SearchForWeaponAction>().OnWeaponFound += (Weapon weapon) => targetWeapon = weapon;
        }

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            if (targetWeapon == null) {
                agent.state.RemoveEffect(Effect.SeesWeapon);
                OnActionFailed();
                return false;
            }

            if (Vector3.Distance(targetWeapon.transform.position, transform.position) > 3) {
                agent.state.RemoveEffect(Effect.InWeaponPickUpRange);
                OnActionFailed();
                return false;
            }

            return true;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() PickupWeaponAction");
            timer = 0;
        }

        public override void UpdateAction() {
            if (targetWeapon == null) {
                OnActionFailed();
                return;
            }

            timer += Time.deltaTime;
            if (timer >= duration)
                OnActionCompleted();
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() PickupWeaponAction");
            Destroy(targetWeapon.gameObject);
            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() PickupWeaponAction ");
            agent.OnActionFailed(this);
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() PickupWeaponAction");
        }
    }
}
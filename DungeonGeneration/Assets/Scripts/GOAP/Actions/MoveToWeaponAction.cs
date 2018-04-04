using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class MoveToWeaponAction : Action {

        private Weapon weapon;
        private float stoppingDistance;

        private void Start() {
            GetComponent<SearchForWeaponAction>().OnWeaponFound += (Weapon weapon) => this.weapon = weapon;
            stoppingDistance = GetComponent<AgentAI>().pickupDistance;
        }

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            if (weapon == null) {
                OnActionFailed();
                return false;
            }

            return true;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() MoveToWeapon " + weapon.name);
        }

        public override void UpdateAction() {
            agent.controller.SetDestination(weapon.transform.position);

            if (Vector3.Distance(agent.transform.position, weapon.transform.position) <= stoppingDistance) {
                agent.controller.StopMoving();
                OnActionCompleted();
            }
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() MoveToWeapon ");
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() MoveToWeapon ");
            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() MoveToWeapon ");
            agent.OnActionFailed(this);
        }
    }
}
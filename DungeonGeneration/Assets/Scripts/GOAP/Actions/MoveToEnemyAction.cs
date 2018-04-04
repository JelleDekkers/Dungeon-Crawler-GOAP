using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class MoveToEnemyAction : Action {

        private Agent target;
        private float stoppingDistance;

        private void Start() {
            stoppingDistance = GetComponent<AgentAI>().pickupDistance;
        }

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            if (target == null) {
                OnActionFailed();
                return false;
            }

            return true;
        }

        public override void OnEnterAction() {
            target = GetComponent<AgentAI>().aggroTarget;
            Debug.Log("OnEnterAction() moveToTarget " + target.name);
        }

        public override void UpdateAction() {
            agent.controller.SetDestination(target.transform.position);
            if (Vector3.Distance(agent.transform.position, target.transform.position) <= stoppingDistance) {
                agent.controller.StopMoving();
                OnActionCompleted();
            }
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() moveToTarget ");
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() moveToTarget ");
            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() moveToTarget ");
            agent.OnActionFailed(this);
        }
    }
}
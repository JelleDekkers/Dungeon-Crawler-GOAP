using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class UseWeaponAction : Action {

        private Agent target;
        private bool attacking;

        public override void Initialize(GOAPAgent goapAgent) {
            this.agent = goapAgent;
            GetComponent<AgentAI>().OnAggroed += (Agent agent) => target = agent;
        }

        public override bool IsViable() {
            if (target == null) {
                agent.state.RemoveEffect(Effect.Aggroed);
                OnActionFailed();
                return false;
            }

            if (Vector3.Distance(target.transform.position, transform.position) > agent.controller.attackRaycastLength) {
                agent.state.RemoveEffect(Effect.InAttackRange);
                OnActionFailed();
                return false;
            }
            return target != null;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() UseWeaponAction");
        }

        public override void UpdateAction() {
            if (target.HealthPoints > 0) {
                if (!attacking)
                    StartCoroutine(Attack());
            } else {
                OnActionCompleted();
            }
        }

        private IEnumerator Attack() {
            float attackTime = Random.Range(1, 2);
            float timer = 0;

            attacking = true;

            agent.controller.Attack();

            while (timer < attackTime) {
                timer += Time.deltaTime;
                yield return null;
            }

            attacking = false;
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() UseWeaponAction");
            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() UseWeaponAction");
            agent.OnActionFailed(this);
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() UseWeaponAction");
        }
    }
}

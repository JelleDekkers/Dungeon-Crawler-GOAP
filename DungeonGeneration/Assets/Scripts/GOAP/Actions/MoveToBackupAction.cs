using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class MoveToBackupAction : Action {

        private Skeleton targetBackup;

        private void Start() {
            GetComponent<SearchForBackupAction>().OnBackupFound += (Skeleton backup) => targetBackup = backup;
        }

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            if (targetBackup == null) {
                agent.state.RemoveEffect(Effect.SeesBackup);
                OnActionFailed();
                return false;
            }

            return true;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() MoveToBackupAction");
        }

        public override void UpdateAction() {
            agent.controller.SetDestination(targetBackup.transform.position);

            if (Vector3.Distance(targetBackup.transform.position, transform.position) < agent.controller.stoppingDistance)
                OnActionCompleted();
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() MoveToBackupAction");
            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() MoveToBackupAction ");
            agent.OnActionFailed(this);
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() MoveToBackupAction");
        }
    }
}
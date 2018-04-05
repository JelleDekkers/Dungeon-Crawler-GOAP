using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class SearchForBackupAction : Action {

        public System.Action<Skeleton> OnBackupFound;

        private Skeleton[] possibleBackup;
        private Skeleton backup;

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            return true;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() SearchForBackupAction ");

            // TODO: nettere manier
            possibleBackup = FindObjectsOfType<Skeleton>();
        }

        public override void UpdateAction() {
            if (possibleBackup.Length > 0) {
                backup = possibleBackup.GetNearest(transform) as Skeleton;
                OnActionCompleted();
            } else {
                OnActionFailed();
            }
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() SearchForBackupAction ");
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() SearchForBackupAction ");

            if (OnBackupFound != null)
                OnBackupFound(backup);

            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() SearchForBackupAction ");
            agent.OnActionFailed(this);
        }
    }
}
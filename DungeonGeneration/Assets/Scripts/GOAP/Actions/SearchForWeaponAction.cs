using System;
using UnityEngine;

namespace GOAP {
    public class SearchForWeaponAction : Action {

        public Weapon[] weapons;
        public Action<Weapon> OnWeaponFound;

        private Weapon weapon;

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
        }

        public override bool IsViable() {
            return true;
        }

        public override void OnEnterAction() {
            Debug.Log("OnEnterAction() SearchForWeaponAction ");

            // TODO: nettere manier
            weapons = FindObjectsOfType<Weapon>();
        }

        public override void UpdateAction() {
            if (weapons.Length > 0) {
                weapon = weapons[0];
                OnActionCompleted();
            } else {
                OnActionFailed();
            }
        }

        public override void OnExitAction() {
            Debug.Log("OnExitAction() SearchForWeaponAction ");
        }

        public override void OnActionCompleted() {
            Debug.Log("OnActionCompleted() SearchForWeaponAction ");

            if (OnWeaponFound != null)
                OnWeaponFound(weapon);

            agent.OnActionCompleted(this);
        }

        public override void OnActionFailed() {
            Debug.Log("OnActionFailed() SearchForWeaponAction ");
            agent.OnActionFailed(this);
        }
    }
}
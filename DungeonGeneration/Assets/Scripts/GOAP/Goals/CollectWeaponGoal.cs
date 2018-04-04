using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class CollectWeaponGoal : Goal {

        public int priorityOnAggroed = 50;

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
            GetComponent<AgentAI>().OnAggroed += OnAggroed;
        }

        private void OnAggroed(Agent player) {
            priority = priorityOnAggroed;
        }

        public override bool IsViable(State state) {
            return state.CheckIfEffectsArePresent(conditions);
        }

        public override void OnGoalCompleted() {
            Debug.Log("OnGoalCompleted() collectWeaponGoal");
        }
    }
}
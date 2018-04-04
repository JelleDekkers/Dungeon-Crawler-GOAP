using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class KillEnemyGoal : Goal {

        public int priorityOnAggroed = 100;
        private Agent target;
        private AgentAI agentAI;

        public override void Initialize(GOAPAgent agent) {
            this.agent = agent;
            agentAI = GetComponent<AgentAI>();
            agentAI.OnAggroed += OnAggroed;
        }

        private void OnAggroed(Agent player) {
            priority = priorityOnAggroed;
            target = player;
        }

        public override bool IsViable(State state) {
            if (Vector3.Distance(target.transform.position, transform.position) > agentAI.maxChaseDistance)
                state.RemoveEffect(Effect.SeesWeapon);
            return state.CheckIfEffectsArePresent(conditions);
        }

        public override void OnGoalCompleted() {
            Debug.Log("OnGoalCompleted() KillEnemyGoal");
        }
    }
}

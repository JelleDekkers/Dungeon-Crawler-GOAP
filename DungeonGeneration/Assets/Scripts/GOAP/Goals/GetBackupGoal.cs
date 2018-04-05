using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GetBackupGoal : Goal {

        public int priorityOnAggroed = 200;

        public override void Initialize(GOAPAgent goapAgent) {
            this.agent = goapAgent;
            GetComponent<AgentAI>().OnAggroed += (Agent agent) => priority = priorityOnAggroed;
        }

        public override bool IsViable(State state) {
            return state.CheckIfEffectsArePresent(conditions);
        }

        public override void OnGoalCompleted() { }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GOAP {
    public abstract class Goal : MonoBehaviour {

        public int priority;

        public List<EffectState> conditions;
        public List<EffectState> effects;
        public Stack<Action> actionStack = new Stack<Action>();

        protected GOAPAgent agent;

        public abstract void Initialize(GOAPAgent agent);
        public abstract bool IsViable(State state);
        public abstract void OnGoalCompleted();
    }
}
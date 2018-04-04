using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public abstract class Action : MonoBehaviour {
        public List<EffectState> conditions;
        public List<EffectState> effects;
        public int cost;

        protected GOAPAgent agent;

        public abstract void Initialize(GOAPAgent agent);
        public abstract bool IsViable();

        public abstract void UpdateAction();
        public abstract void OnEnterAction();
        public abstract void OnExitAction();
        public abstract void OnActionCompleted();
        public abstract void OnActionFailed();
    }

}
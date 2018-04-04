using UnityEngine;
using UnityEngine.AI;

namespace GOAP {

    public abstract class GOAPAgent : MonoBehaviour {

        public State state;
        public Goal currentGoal;
        public Action currentAction;
        public Goal fallbackGoal;

        [HideInInspector]
        public Goal[] allGoals;
        [HideInInspector]
        public Action[] allActions;

        public AIController controller;

        protected virtual void Start() {
            allActions = GetComponents<Action>();
            allGoals = GetComponents<Goal>();

            controller = GetComponent<AIController>();
        }

        protected void InitializeActionsAndGoals() {
            foreach (Action action in allActions)
                action.Initialize(this);

            foreach (Goal goal in allGoals)
                goal.Initialize(this);
        }

        protected virtual void Update() {
            ExecuteGoal();
        }

        public void ExecuteGoal() {
            if (currentGoal == null) {
                ReplanGoal();
            } else if (currentAction == null) {
                GetNextAction();
            } else if (currentAction.IsViable() && currentGoal.IsViable(state)) {
                currentAction.UpdateAction();
            } else {
                ReplanGoal();
            }
        }

        public void OnActionCompleted(Action action) {
            state.AddEffectsToState(action.effects);
            action.OnExitAction();
            GetNextAction();
        }

        public void OnActionFailed(Action action) {
            action.OnExitAction();
            currentAction = null;
        }

        public void GetNextAction() {
            if (currentGoal != null && currentGoal.actionStack.Count > 0) {
                currentAction = currentGoal.actionStack.Pop();
                currentAction.OnEnterAction();
            } else {
                OnGoalCompleted();
            }
        }

        protected virtual void OnGoalCompleted() {
            currentGoal.OnGoalCompleted();
            ReplanGoal();
        }

        public void ReplanGoal() {
            if (currentAction != null)
                currentAction.OnExitAction();
            currentAction = null;
            currentGoal = null;
            Planner.PlanActions(state, allActions, allGoals, fallbackGoal, out currentGoal);
        }
    }
}
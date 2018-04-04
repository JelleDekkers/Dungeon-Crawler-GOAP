using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GOAP {

    public class Planner : MonoBehaviour {

        public static void PlanActions(State playerState, Action[] availableActions, Goal[] availableGoals, Goal fallbackGoal, out Goal chosenGoal) {
            chosenGoal = null;
            foreach (Goal goal in availableGoals.OrderByDescending(g => g.priority).ToList()) {
                if (goal.IsViable(playerState)) {
                    TryPlanWithAStar(goal, playerState, availableActions, ref chosenGoal);
                    if (chosenGoal != null)
                        return;
                }
            }

            Debug.LogWarning("No path found!");
            chosenGoal = fallbackGoal;
        }

        /// <summary>
        /// Based on Pseudocode from wiki: https://en.wikipedia.org/wiki/A*_search_algorithm
        /// Loop Safe!
        /// </summary>
        /// <param name="goalToReach"></param>
        /// <param name="currentState"></param>
        /// <param name="availableActions"></param>
        /// <param name="chosenGoal"></param>
        public static void TryPlanWithAStar(Goal goalToReach, State currentState, Action[] availableActions, ref Goal chosenGoal) {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node>();

            State state = new State();
            state.effect = currentState.effect;
            openList.Add(new Node(null, null, state, 0));

            Dictionary<Effect, int> gScore = new Dictionary<Effect, int>();
            gScore.Add(openList[0].state.effect, 0);

            float loopsMade = 0;
            int maxLoops = 50;

            while (openList.Count > 0 && loopsMade < maxLoops) {
                loopsMade++;
                Node curNode = openList.OrderByDescending((t) => t.cumulativeCost).ToList().LastOrDefault();

                if (curNode.state.CheckIfEffectsArePresent(goalToReach.effects)) {
                    chosenGoal = goalToReach;
                    ReconstructPath(curNode, ref chosenGoal);
                    return;
                }

                openList.Remove(curNode);
                closedList.Add(curNode);

                foreach (Action action in availableActions) {
                    if (curNode.state.CheckIfEffectsArePresent(action.conditions)) {
                        Node n = curNode.CloneNode();
                        n.parentNode = curNode;
                        n.cumulativeCost += action.cost;
                        n.actionForThisNode = action;
                        n.state.AddEffectsToState(action.effects);

                        if (closedList.Contains(n, new NodeComparer()))
                            continue;

                        int tentativeScore = n.cumulativeCost;
                        if (!openList.Contains(n, new NodeComparer()))
                            openList.Add(n);
                        else if (tentativeScore >= gScore[n.state.effect])
                            continue;

                        gScore[n.state.effect] = tentativeScore;
                    }
                }
            }
            return;
        }

        public static void ReconstructPath(Node endNode, ref Goal chosenGoal) {
            chosenGoal.actionStack = new Stack<Action>();
            while (endNode.parentNode != null && endNode.actionForThisNode != null) {
                chosenGoal.actionStack.Push(endNode.actionForThisNode);
                endNode = endNode.parentNode;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    public class Node {
        public Node parentNode;
        public Action actionForThisNode;
        public State state;
        public int cumulativeCost;

        public Node(Node parentNode, Action action, State state, int cumulativeCost) {
            this.state = state;
            actionForThisNode = action;
            this.parentNode = parentNode;
            this.cumulativeCost = cumulativeCost;
        }

        public void AddEffectsToState(List<EffectState> effects) {
            state.AddEffectsToState(effects);
        }

        public Node CloneNode() {
            Node n = (Node)this.MemberwiseClone();
            n.state = new State();
            n.state.effect = this.state.effect;
            return n;
        }
    }

    public class NodeComparer : IEqualityComparer<Node> {
        public bool Equals(Node n1, Node n2) {
            return n1.state.CompareState(n2.state);
        }

        public int GetHashCode(Node n) {
            return n.GetHashCode();
        }
    }

}
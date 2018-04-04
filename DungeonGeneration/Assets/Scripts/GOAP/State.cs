using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {

    [System.Serializable]
    public class State {

        [BitMask(typeof(Effect))]
        public Effect effect;

        public void AddEffectsToState(List<EffectState> effects) {
            foreach (EffectState efState in effects) {
                if (efState.value) {
                    AddEffect(efState.effect);
                } else {
                    RemoveEffect(efState.effect);
                }
            }
        }

        public void AddEffect(Effect effect) {
            this.effect |= effect;
        }

        public void RemoveEffect(Effect effect) {
            this.effect &= ~effect;
        }

        public bool CheckIfEffectsArePresent(List<EffectState> effects) {
            bool result = true;
            foreach (EffectState efState in effects) {
                result &= efState.value ? CheckIfEffectIsPresent(efState.effect) : CheckIfEffectIsNotPresent(efState.effect);
            }
            return result;
        }

        public bool CheckIfEffectIsPresent(Effect effect) {
            return (this.effect & effect) != 0;
        }

        public bool CheckIfEffectIsNotPresent(Effect effect) {
            return !((this.effect & effect) != 0);
        }

        public bool CompareState(State otherState) {
            return (effect == otherState.effect);
        }
    }
}
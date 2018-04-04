namespace GOAP {

    [System.Serializable]
    public struct EffectState {
        public Effect effect;
        public bool value;

        public EffectState(Effect effect, bool value) {
            this.effect = effect;
            this.value = value;
        }
    }
}
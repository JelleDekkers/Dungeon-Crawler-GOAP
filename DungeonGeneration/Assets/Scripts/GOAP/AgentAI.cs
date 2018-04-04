using UnityEngine;
using System;
using GOAP;

public class AgentAI : GOAPAgent {

    public DetectionCollider detector;
    public Agent aggroTarget;
    public Action<Agent> OnAggroed;
    public float pickupDistance = 0.2f;
    public float maxChaseDistance = 20;

    protected override void Start() {
        base.Start();
        InitializeActionsAndGoals();
        detector.OnPlayerSpotted += SetAggroed;

        GetComponent<Agent>().OnDeath += () => {
            enabled = false;
            Destroy(gameObject, 5f);
        };
    }

    private void SetAggroed(Agent player) {
        aggroTarget = player;
        state.AddEffect(Effect.Aggroed);

        if (OnAggroed != null)
            OnAggroed(player);

        ReplanGoal();

        detector.OnPlayerSpotted -= SetAggroed; 
    }
}

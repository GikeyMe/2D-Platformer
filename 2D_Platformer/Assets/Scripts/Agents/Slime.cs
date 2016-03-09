using UnityEngine;
using System.Collections;
using System;

public class Slime : Agent {

    private bool PlayerNearby;

    // Use this for initialization
    protected override void Start () {
        base.Start();
    }
	
	void FixedUpdate () {
        if (Alive())
        {
            PlayerNearby = LookForPlayer();
            if (!Patrol && !PlayerNearby && !(activeState is IdleState))
                SetState(new IdleState());
            else if (Patrol && !PlayerNearby && !(activeState is PatrolState))
                SetState(new PatrolState());
            else if (PlayerMeleeRange() && !(activeState is MeleeAttackState))
                SetState(new MeleeAttackState());
            else if (PlayerNearby && !PlayerMeleeRange() && !(activeState is ChaseState))
                SetState(new ChaseState());

            activeState.Act();
        }
    }

}

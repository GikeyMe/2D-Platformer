using UnityEngine;
using System.Collections;

public class Bat : Agent {

    private bool PlayerNearby;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
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

    protected override bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 25)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 10)
                return true;
        }
        return false;
    }
}

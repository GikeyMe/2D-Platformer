using UnityEngine;
using System.Collections;

public class Worm : Agent {

    private bool PlayerNearby;
    private bool triggeredDeath;

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
            else if (PlayerNearby && !IsPlayerReachable() && !(activeState is HideState))
                SetState(new HideState());
            else if (PlayerMeleeRange() && !(activeState is MeleeAttackState))
                SetState(new MeleeAttackState());
            else if (PlayerNearby && !PlayerMeleeRange() && IsPlayerReachable() && !(activeState is ChaseState))
                SetState(new ChaseState());

            activeState.Act();
        }
        if (!Alive() && !triggeredDeath)
        {
            triggeredDeath = true;
            AgentAnimator.SetTrigger("Death");
        }
    }

    protected override bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 25)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 5)
                return true;
        }
        return false;
    }
}

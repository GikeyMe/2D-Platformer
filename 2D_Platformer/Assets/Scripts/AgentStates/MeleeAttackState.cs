using UnityEngine;
using System.Collections;
using System;

public class MeleeAttackState : IAgentState
{
    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;
    private GameObject player;
    private bool RecentlyAttacked;

    public void Activate(Agent agent)
    {
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        player = GameObject.Find("Player");
        Debug.Log("Melee State");
    }

    public void Act()
    {
        currentAgent.MeleeHitBox.enabled = false;
        if (RecentlyAttacked)
        {
            MoveFromPlayer();
        }
        else
        {
            Attack();
        }

    }

    private void MoveFromPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) > 3)
        {
            RecentlyAttacked = false;
        }
        if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop())
        {
            AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
        }
        if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart())
        {
            AgentRigidbody.velocity = new Vector2(1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
        }
    }

    private void MoveToPlayer()
    {
        if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop())
        {
            AgentRigidbody.velocity = new Vector2(1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
            currentAgent.Flip(1);
        }
        if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart())
        {
            AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
            currentAgent.Flip(-1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        AgentAnimator.SetFloat("speed", 1);
    }

    private void Attack()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 1.5)
        {
            currentAgent.MeleeHitBox.enabled = true;
            AgentRigidbody.velocity = new Vector2((float)0.000001, AgentRigidbody.velocity.y);  //Need a tiny velocity to trigger the OnTriggerEnter event in Player.
            AgentRigidbody.velocity = new Vector2(0, AgentRigidbody.velocity.y);
            RecentlyAttacked = true;
        }
        else
            MoveToPlayer();
    }


}

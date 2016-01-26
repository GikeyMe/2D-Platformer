using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IAgentState
{
    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;

    public void Activate(Agent agent)
    {
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        Debug.Log("Patrol State");
    }

    public void Act()
    {
        if (currentAgent.isFacingRight() && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop())
            AgentRigidbody.velocity = new Vector2(1 * currentAgent.getpatrolSpeed(), AgentRigidbody.velocity.y);
        if (!currentAgent.isFacingRight() && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart())
            AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getpatrolSpeed(), AgentRigidbody.velocity.y);
        if (AgentRigidbody.transform.position.x >= currentAgent.getxPatrolStop())
        {
            currentAgent.Flip(-1);
        }
        if (AgentRigidbody.transform.position.x <= currentAgent.getxPatrolStart())
        {
            currentAgent.Flip(1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        AgentAnimator.SetFloat("speed", 1);
    }


}

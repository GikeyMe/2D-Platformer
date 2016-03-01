using UnityEngine;
using System.Collections;
using System;

public class PhaseChangeState : IAgentState {

    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;

    public void Activate(Agent agent)
    {
        Debug.Log("PhaseChangeState");
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        if (currentAgent is Boss || currentAgent is BossThree)
            AgentAnimator.SetBool("Throwing", false);
    }

    public void Act()
    {        
        currentAgent.Flip(1);
        AgentRigidbody.velocity = new Vector2(2 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);  // run right towards next platform
        AgentAnimator.SetFloat("speed", 1);
    }
}

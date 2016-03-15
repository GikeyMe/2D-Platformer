﻿using UnityEngine;
using System.Collections;
using System;

public class IdleState : IAgentState
{
    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;

    public void Activate(Agent agent)
    {
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        Debug.Log("Idle State");
    }

    public void Act()
    {
        if (!(currentAgent is Bat))
        {
            if (AgentRigidbody.velocity.x == 0)
                AgentAnimator.SetFloat("speed", 0);
        }
        if(currentAgent is Bat)
        {
            AgentRigidbody.velocity = new Vector2(0, 0);
        }
    }
}

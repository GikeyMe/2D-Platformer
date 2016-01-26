﻿using UnityEngine;
using System.Collections;
using System;

public class ChaseState : IAgentState
{
    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;
    private GameObject player;

    public void Activate(Agent agent)
    {
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        player = GameObject.Find("Player");
        Debug.Log("Chase State");
    }

    public void Act()
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


}

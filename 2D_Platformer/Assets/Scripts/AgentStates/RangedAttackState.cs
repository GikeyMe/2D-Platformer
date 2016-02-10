using UnityEngine;
using System.Collections;
using System;

public class RangedAttackState : IAgentState {

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
        Debug.Log("Ranged State");
    }

    public void Act()
    {
        if (currentAgent is Boss)
        {
            currentAgent.Flip(-1);
            AgentAnimator.SetBool("Throwing", true);
        }
    }
}

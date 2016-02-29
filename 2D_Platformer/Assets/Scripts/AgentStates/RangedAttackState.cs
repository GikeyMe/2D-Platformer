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
        AgentAnimator.SetFloat("speed", 0);
        if (player.transform.position.x < AgentRigidbody.transform.position.x)
            currentAgent.Flip(-1);
        if (player.transform.position.x > AgentRigidbody.transform.position.x)
            currentAgent.Flip(1);
        if (currentAgent is Boss)
            AgentAnimator.SetBool("Throwing", true);
        if (currentAgent is Crawler)
            AgentAnimator.SetBool("Shooting", true);
    }
}

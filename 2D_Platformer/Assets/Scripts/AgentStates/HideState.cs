using UnityEngine;
using System.Collections;
using System;

public class HideState : IAgentState {

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
        Debug.Log("Hide State");
    }

    public void Act()
    {
        MoveFromPlayer();
    }

    private void MoveFromPlayer()
    {
        if ((player.transform.position.x > AgentRigidbody.transform.position.x ) && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart())
        {
            AgentRigidbody.velocity = new Vector2((float)(-1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
            if (currentAgent.isFacingRight())
                currentAgent.Flip(-1);
        }
        if ((player.transform.position.x < AgentRigidbody.transform.position.x ) && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop())
        {
            AgentRigidbody.velocity = new Vector2((float)(1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
            if (!currentAgent.isFacingRight())
                currentAgent.Flip(1);
        }
    }


}

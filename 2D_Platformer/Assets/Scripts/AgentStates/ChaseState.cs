using UnityEngine;
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
        if (currentAgent is Slime || currentAgent is Worm)
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

        else if (currentAgent is Boss)
        {
            //should probably introduce similar check for worm so he cant move horizontally while playing attack animation.
            if (!(AgentAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Melee")))
            {
                if (player.transform.position.x > AgentRigidbody.transform.position.x)
                {
                    AgentRigidbody.velocity = new Vector2(1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
                    currentAgent.Flip(1);
                }
                if (player.transform.position.x < AgentRigidbody.transform.position.x)
                {
                    AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
                    currentAgent.Flip(-1);
                }
                //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
                AgentAnimator.SetFloat("speed", 1);
            }
        }

        else if (currentAgent is Bat)
        {
            if (player.transform.position.x > AgentRigidbody.transform.position.x)
            {
                AgentRigidbody.velocity = new Vector2(1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
                currentAgent.Flip(1);
            }
            if (player.transform.position.x < AgentRigidbody.transform.position.x)
            {
                AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
                currentAgent.Flip(-1);
            }
            if ((player.transform.position.y - AgentRigidbody.transform.position.y) < -2)
            {
                AgentRigidbody.velocity = new Vector2(AgentRigidbody.velocity.x, -1 * currentAgent.getrunSpeed());
            }
            if ((player.transform.position.y - AgentRigidbody.transform.position.y) > 2)
            {
                AgentRigidbody.velocity = new Vector2(AgentRigidbody.velocity.x, 1 * currentAgent.getrunSpeed());
            }
        }
    }


}

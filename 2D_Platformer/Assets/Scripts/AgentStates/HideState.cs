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
        if (currentAgent is Seeker)                     //need seperate code so that seeker doesn't accidentally trigger powerups when hiding
        {
            if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart() + 10)   // run away to start of patrol point
            {
                AgentRigidbody.velocity = new Vector2((float)(-1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
                AgentAnimator.SetFloat("speed", 1);
                if (currentAgent.isFacingRight())
                    currentAgent.Flip(-1);
            }
            else if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x <= currentAgent.getxPatrolStart() + 10)  // reached start so turn around and idle
            {
                AgentAnimator.SetFloat("speed", 0);
                currentAgent.Flip(1);
            }

            if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop() - 10)  //run away to stop of patrol point
            {
                AgentRigidbody.velocity = new Vector2((float)(1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
                AgentAnimator.SetFloat("speed", 1);
                if (!currentAgent.isFacingRight())
                    currentAgent.Flip(1);
            }
            else if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x >= currentAgent.getxPatrolStop() - 10)  // reached stop so turn around and idle
            {
                AgentAnimator.SetFloat("speed", 0);
                currentAgent.Flip(-1);
            }
        }

        else if (!(currentAgent is Seeker))
        {

            if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > currentAgent.getxPatrolStart() + 3)   // run away to start of patrol point
            {
                AgentRigidbody.velocity = new Vector2((float)(-1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
                AgentAnimator.SetFloat("speed", 1);
                if (currentAgent.isFacingRight())
                    currentAgent.Flip(-1);
            }
            else if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x <= currentAgent.getxPatrolStart() + 3)  // reached start so turn around and idle
            {
                AgentAnimator.SetFloat("speed", 0);
                currentAgent.Flip(1);
            }



            if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < currentAgent.getxPatrolStop() - 3)  //run away to stop of patrol point
            {
                AgentRigidbody.velocity = new Vector2((float)(1.5 * currentAgent.getrunSpeed()), AgentRigidbody.velocity.y);
                AgentAnimator.SetFloat("speed", 1);
                if (!currentAgent.isFacingRight())
                    currentAgent.Flip(1);
            }
            else if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x >= currentAgent.getxPatrolStop() - 3)  // reached stop so turn around and idle
            {
                AgentAnimator.SetFloat("speed", 0);
                currentAgent.Flip(-1);
            }
        }
    }


}

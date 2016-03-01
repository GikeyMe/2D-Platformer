using UnityEngine;
using System.Collections;
using System;

public class SeekPowerState : IAgentState {

    private Agent currentAgent;
    private Rigidbody2D AgentRigidbody;
    private Animator AgentAnimator;
    private GameObject player;
    private GameObject[] powerups;
    private GameObject chosenpowerup;

    public void Activate(Agent agent)
    {
        currentAgent = agent;
        AgentRigidbody = currentAgent.GetComponent<Rigidbody2D>();
        AgentAnimator = currentAgent.GetComponent<Animator>();
        player = GameObject.Find("Player");
        powerups = GameObject.FindGameObjectsWithTag("BlueDiamond");
        Debug.Log("Seek Power State");
        if (currentAgent is Boss || currentAgent is BossThree)
            AgentAnimator.SetBool("Throwing", false);
    }

    public void Act()
    {
        if (currentAgent is BossTwo || currentAgent is BossThree)
        {
            goToPowerUp((GameObject)powerups.GetValue(0));
            return;
        }

        foreach (GameObject powerup in powerups)
        {
            if (isPowerUpReachable(powerup))
            {
                chosenpowerup = powerup;
                break;
            }
        }
        if (chosenpowerup != null)
        {
            goToPowerUp(chosenpowerup);
        }
    }

    private bool isPowerUpReachable(GameObject powerup)
    {
        if ((powerup.transform.position.x > currentAgent.getxPatrolStart()) && (powerup.transform.position.x < currentAgent.getxPatrolStop())
            && (Mathf.Abs(powerup.transform.position.y - currentAgent.transform.position.y) < 5))
        {
            return true;
        }
        else
            return false;
    }

    private void goToPowerUp(GameObject powerup)
    {
        if (powerup.transform.position.x > AgentRigidbody.transform.position.x)
        {
            AgentRigidbody.velocity = new Vector2(1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
            currentAgent.Flip(1);
        }
        if (powerup.transform.position.x < AgentRigidbody.transform.position.x)
        {
            AgentRigidbody.velocity = new Vector2(-1 * currentAgent.getrunSpeed(), AgentRigidbody.velocity.y);
            currentAgent.Flip(-1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        AgentAnimator.SetFloat("speed", 1);
    }
}

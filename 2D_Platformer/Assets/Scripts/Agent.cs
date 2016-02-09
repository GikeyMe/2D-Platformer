using UnityEngine;
using System.Collections;

public abstract class Agent : MonoBehaviour {

    private bool facingRight;
    protected Animator AgentAnimator;
    protected Rigidbody2D AgentRigidbody;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float xPatrolStart;
    [SerializeField]
    private float xPatrolStop;
    [SerializeField]
    protected bool Patrol;
    [SerializeField]
    public BoxCollider2D MeleeHitBox;
    
    protected GameObject player;

    protected IAgentState activeState;
    [SerializeField]
    private int hitpoints;



    public bool isFacingRight()
    {
        return facingRight;
    }

    public float getpatrolSpeed()
    {
        return patrolSpeed;
    }

    public float getrunSpeed()
    {
        return runSpeed;
    }

    public float getxPatrolStart()
    {
        return xPatrolStart;
    }

    public float getxPatrolStop()
    {
        return xPatrolStop;
    }

    protected void SetState(IAgentState State)
    {
        activeState = State;
        activeState.Activate(this);                  // pass the agent that is changing states to the necessary state
    }

    protected virtual void Start()
    {
        SetState(new IdleState());
        facingRight = true;                              //we know the slime starts off facing right so set this to true
        AgentRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        AgentAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    protected bool Alive()
    {
        return hitpoints > 0;
    }

    //override as we need to also check y values for bat because it can fly.
    protected bool PlayerMeleeRange()
    {
        if ((Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 3.1) && (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 2.2))
        {
            return true;
        }
        return false;
    }

    protected virtual bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 25)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 3)
                return true;
        }
        return false;
    }

    public void Flip(float direction)
    {
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {

            //negate the x axis scale for the player (i.e. flip the sprite)
            Vector3 agentScale = transform.localScale;
            agentScale.x *= -1;
            transform.localScale = agentScale;

            //need to change this each time we flip the agent otherwise we will lose track of the direction he is facing
            facingRight = !facingRight;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (Alive())
        {
            if (EnterredObject.tag == "Bullet")
            {
                TakeDamage(10);
            }
            if (EnterredObject.tag == "Melee")
            {
                TakeDamage(20);
            }
        }
    }

    private void TakeDamage(int hp)
    {
        hitpoints -= hp;
        if (Alive())
        {
            AgentAnimator.SetTrigger("Damage");
        }
        else
        {
            AgentAnimator.SetTrigger("Death");
        }
    }

    private void DestroyAgent()
    {
        Destroy(gameObject);
    }

    private void ActivateGravity()
    {
        AgentRigidbody.gravityScale = 8;
    }

    private void EnableHitBox()
    {
        MeleeHitBox.enabled = true;
        AgentRigidbody.velocity = new Vector2((float)0.000001, AgentRigidbody.velocity.y);  //Need a tiny velocity to trigger the OnTriggerEnter event in Player.
        AgentRigidbody.velocity = new Vector2(0, AgentRigidbody.velocity.y);
    }

    private void DisableHitBox()
    {
        MeleeHitBox.enabled = false;
    }

    private bool canAgentFly()
    {
        return (this is Bat);
    }

    protected bool IsPlayerReachable()
    {
        if (((player.transform.position.x < xPatrolStart-1.5) || (player.transform.position.x > xPatrolStop+1.5)) && (!(canAgentFly())))
            return false;
        return true;
    }



}

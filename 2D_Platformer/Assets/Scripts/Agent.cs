using UnityEngine;
using System.Collections;

public abstract class Agent : MonoBehaviour {

    private bool facingRight;
    private Animator AgentAnimator;
    private Rigidbody2D AgentRigidbody;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private float xPatrolStart;
    [SerializeField]
    private float xPatrolStop;
    [SerializeField]
    private bool Patrol;
    [SerializeField]
    public BoxCollider2D MeleeHitBox;
    private bool PlayerNearby;
    private GameObject player;

    private IAgentState activeState;
    [SerializeField]
    protected int hitpoints;



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

    private void SetState(IAgentState State)
    {
        activeState = State;
        activeState.Activate(this);                  // pass the agent that is changing states to the necessary state
    }

    // Use this for initialization
   protected virtual void Start () {
        SetState(new IdleState());
        facingRight = true;                              //we know the slime starts off facing right so set this to true
        AgentRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        AgentAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    protected virtual void Update()
    {       
        if (Alive())
        {
            PlayerNearby = LookForPlayer();
            if (!Patrol && !PlayerNearby && !(activeState is IdleState))
                SetState(new IdleState());
            else if (Patrol && !PlayerNearby && !(activeState is PatrolState))
                SetState(new PatrolState());
            else if (PlayerMeleeRange() && !(activeState is MeleeAttackState))
                SetState(new MeleeAttackState());         
            else if (PlayerNearby && !PlayerMeleeRange() && !(activeState is ChaseState))
                SetState(new ChaseState());

            activeState.Act();
        }
    }

    private bool Alive()
    {
        return hitpoints > 0;
    }

    private bool PlayerMeleeRange()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 3.1)
        {
            return true;
        }
        return false;
    }

    private bool LookForPlayer()
    {
        if (Mathf.Abs(GameObject.Find("Player").transform.position.x - AgentRigidbody.transform.position.x) < 30)
        {
            if (Mathf.Abs(GameObject.Find("Player").transform.position.y - AgentRigidbody.transform.position.y) < 10)
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



}

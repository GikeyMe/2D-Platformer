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
    private BoxCollider2D MeleeHitBox;
    private bool PlayerNearby;
    private GameObject player;
    private bool RecentlyAttacked;


    [SerializeField]
    protected int hitpoints;


    // Use this for initialization
   protected virtual void Start () {
        facingRight = true;                              //we know the slime starts off facing right so set this to true
        AgentRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        AgentAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    protected virtual void Update()
    {
        if (Alive())
        {
            if (AgentRigidbody.velocity.x == 0)
                AgentAnimator.SetFloat("speed", 0);

            PlayerNearby = LookForPlayer();

            if (Patrol && !PlayerNearby)
                AgentPatrol();
            if (PlayerNearby  && !RecentlyAttacked)
                MoveToPlayer();
            if (PlayerNearby && RecentlyAttacked)
            {
                MoveFromPlayer();
            }
        }
    }

    private bool Alive()
    {
        return hitpoints > 0;
    }

    private void MoveFromPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) > 3)
        {
            RecentlyAttacked = false;
        }
        if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < xPatrolStop)
        {
            AgentRigidbody.velocity = new Vector2(-1 * runSpeed, AgentRigidbody.velocity.y);
        }
        if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > xPatrolStart)
        {
            AgentRigidbody.velocity = new Vector2(1 * runSpeed, AgentRigidbody.velocity.y);
        }
    }

    private void MoveToPlayer()
    {
        MeleeHitBox.enabled = false;

        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 1.5)
        {
            Attack();    
            return;
        }
        if ((player.transform.position.x > AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x < xPatrolStop)
        {
            AgentRigidbody.velocity = new Vector2(1 * runSpeed, AgentRigidbody.velocity.y);
            Flip(1);
        }
        if ((player.transform.position.x < AgentRigidbody.transform.position.x) && AgentRigidbody.transform.position.x > xPatrolStart)
        {
            AgentRigidbody.velocity = new Vector2(-1 * runSpeed, AgentRigidbody.velocity.y);
            Flip(-1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        AgentAnimator.SetFloat("speed", 1);

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

    private void AgentPatrol()
    {
        if (facingRight && AgentRigidbody.transform.position.x < xPatrolStop)
            AgentRigidbody.velocity = new Vector2(1 * patrolSpeed, AgentRigidbody.velocity.y);
        if (!facingRight && AgentRigidbody.transform.position.x > xPatrolStart)
            AgentRigidbody.velocity = new Vector2(-1 * patrolSpeed, AgentRigidbody.velocity.y);
        if (AgentRigidbody.transform.position.x >= xPatrolStop)
        {
            Flip(-1);
        }
        if (AgentRigidbody.transform.position.x <= xPatrolStart)
        {
            Flip(1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        AgentAnimator.SetFloat("speed", 1);
    }

    private void Flip(float direction)
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


    private void Attack()
    { 
        //otherwise just attack
        MeleeHitBox.enabled = true;
        AgentRigidbody.velocity = new Vector2((float)0.000001, AgentRigidbody.velocity.y);  //Need a tiny velocity to trigger the OnTriggerEnter event in Agent.
        AgentRigidbody.velocity = new Vector2(0, AgentRigidbody.velocity.y);
        RecentlyAttacked = true;
    }
}

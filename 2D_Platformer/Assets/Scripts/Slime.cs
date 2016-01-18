using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour {

    private bool facingRight;
    private Animator SlimeAnimator;
    private Rigidbody2D SlimeRigidbody;
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
    private bool PlayerNearby;

	// Use this for initialization
	void Start () {
        facingRight = true;                              //we know the slime starts off facing right so set this to true
        SlimeRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        SlimeAnimator = GetComponent<Animator>();
    }
	
	void FixedUpdate () {
        if (SlimeRigidbody.velocity.x == 0)
            SlimeAnimator.SetFloat("speed", 0);

        PlayerNearby = LookForPlayer();

        if (Patrol && !PlayerNearby)
          SlimePatrol();
        if (PlayerNearby)
          MoveToPlayer();
	}

    private void MoveToPlayer()
    {
        if (Mathf.Abs(GameObject.Find("Player").transform.position.x - SlimeRigidbody.transform.position.x) < 0.5)
        {
            SlimeRigidbody.velocity = new Vector2(0 * runSpeed, SlimeRigidbody.velocity.y);
            SlimeAnimator.SetFloat("speed", 0);
            return;
        }
        if ((GameObject.Find("Player").transform.position.x > SlimeRigidbody.transform.position.x) && SlimeRigidbody.transform.position.x < xPatrolStop) 
        {
            SlimeRigidbody.velocity = new Vector2(1 * runSpeed, SlimeRigidbody.velocity.y);
            Flip(1);
        }
        if ((GameObject.Find("Player").transform.position.x < SlimeRigidbody.transform.position.x) && SlimeRigidbody.transform.position.x > xPatrolStart)
        {
            SlimeRigidbody.velocity = new Vector2(-1 * runSpeed, SlimeRigidbody.velocity.y);
            Flip(-1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        SlimeAnimator.SetFloat("speed", 1); 

    }

    private bool LookForPlayer()
    {
        if (Mathf.Abs(GameObject.Find("Player").transform.position.x - SlimeRigidbody.transform.position.x) < 30)
        {
            if (Mathf.Abs(GameObject.Find("Player").transform.position.y - SlimeRigidbody.transform.position.y) < 10)
              return true;
        }
        return false;
    }

    private void SlimePatrol()
    {
        if (facingRight && SlimeRigidbody.transform.position.x < xPatrolStop)
          SlimeRigidbody.velocity = new Vector2(1 * patrolSpeed, SlimeRigidbody.velocity.y);
        if (!facingRight && SlimeRigidbody.transform.position.x > xPatrolStart)
            SlimeRigidbody.velocity = new Vector2(-1 * patrolSpeed, SlimeRigidbody.velocity.y);
        if (SlimeRigidbody.transform.position.x >= xPatrolStop)
        {
            Flip(-1);
        }
        if (SlimeRigidbody.transform.position.x <= xPatrolStart)
        {
            Flip(1);
        }
        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        SlimeAnimator.SetFloat("speed", 1);  
    }

    private void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {

            //negate the x axis scale for the player (i.e. flip the sprite)
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            //need to change this each time we flip the player otherwise we will lose track of the direction he is facing
            facingRight = !facingRight;
        }
    }
}

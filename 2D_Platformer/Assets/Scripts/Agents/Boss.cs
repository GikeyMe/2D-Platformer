using UnityEngine;
using System.Collections;

public class Boss : Agent {

    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField]
    private BossExitDoor exitDoor;

    [SerializeField]
    private Transform KnifePosition;

    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundCollideRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private HealthBar myHealthBar;

    private bool onGround;
    private bool PlayerNearby;
    private bool FirstPhase;
    private bool SecondPhase;
    private bool ThirdPhase;
    private bool FourthPhase;
    private bool SecondPhaseReached;
    private bool ThirdPhaseReached;
    private bool FourthPhaseReached;

    // Use this for initialization
    protected override void Start () {
      //  onGround = CheckGrounded();
        base.Start();
        FirstPhase = true;
    }

    void Update()
    {
        myHealthBar.UpdateHealth(hitpoints, 400f);
        if (hitpoints < 300 && !SecondPhaseReached)
        {
            FirstPhase = false;
            SecondPhase = true;
        }
        else if (hitpoints < 200 && !ThirdPhaseReached)
        {
            ThirdPhase = true;
        }
        else if (hitpoints < 100 && !FourthPhaseReached)
        {
            FourthPhase = true;
        }
        else if (hitpoints <= 0)
        {
            if (!(activeState is PhaseChangeState))
                SetState(new PhaseChangeState());
            activeState.Act();
            exitDoor.OpenDoor();
        }

        if (Alive())
        {
            if (FirstPhase)
            {
                PlayerNearby = LookForPlayer();
                if (SecondPhase || ThirdPhase || FourthPhase)
                {
                    if(!(activeState is PhaseChangeState))
                        SetState(new PhaseChangeState());
                }
                else if (!Patrol && !PlayerNearby && !(activeState is IdleState))
                    SetState(new IdleState());                                          //can possibly remove idle and patrol states here.
                else if (Patrol && !PlayerNearby && !(activeState is PatrolState))
                    SetState(new PatrolState());
                else if (PlayerMeleeRange() && !(activeState is MeleeAttackState))
                    SetState(new MeleeAttackState());
                else if (PlayerNearby && !PlayerMeleeRange() && !(activeState is ChaseState))
                    SetState(new ChaseState());
            }
            else
            {
                if (SecondPhase || ThirdPhase || FourthPhase)
                {
                    if (!(activeState is PhaseChangeState))
                        SetState(new PhaseChangeState());
                }
                else if (PlayerMeleeRange() && !(activeState is MeleeAttackState))
                {
                    AgentAnimator.SetBool("Throwing", false);
                    SetState(new MeleeAttackState());                    
                }
                else if (!(activeState is RangedAttackState))
                    SetState(new RangedAttackState());
            }

            activeState.Act();
        }
    }

    public override void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (EnterredObject.tag == "JumpNotify")
            Jump();
        if (EnterredObject.tag == "PhaseTwo")
        {
            SecondPhaseReached = true;
            SecondPhase = false;
            Debug.Log("Second Phase Reached.");
            AgentAnimator.SetFloat("speed", 0);
        }
        if (EnterredObject.tag == "PhaseThree")
        {
            ThirdPhaseReached = true;
            ThirdPhase = false;
            AgentAnimator.SetFloat("speed", 0);
        }
        if (EnterredObject.tag == "PhaseFour")
        {
            FourthPhaseReached = true;
            FourthPhase = false;
            AgentAnimator.SetFloat("speed", 0);
        }

        if (EnterredObject.tag == "BossDespawn" && hitpoints <= 0)
            Destroy(gameObject);
        base.OnTriggerEnter2D(EnterredObject);
    }

    protected override bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 40)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 5)
                return true;
        }
        return false;
    }

    void FixedUpdate()
    {       
        onGround = CheckGrounded();
        if (AgentRigidbody.velocity.y < 0)  //if player character is falling and not on a moving platform
        {
            AgentAnimator.SetBool("Falling", true); //play falling animation. This can't be a trigger because we are unsure how far the player character is falling and so the animation has to be played 
        }                                            // until explicitly told to stop. (i.e boolean is false again)
        HandleLayers();
    }

    private void Jump()
    {
        if ((onGround))  //if the player presses space and we aren't already jumping or falling
        {
            onGround = false;
            AgentRigidbody.AddForce(new Vector2(0, jumpSpeed));    // make the player character jump (i.e apply a positive force on the y axis)
            AgentAnimator.SetTrigger("Jump");                         // Trigger the jump animation
        }
    }

    private bool CheckGrounded()
    {
        if (AgentRigidbody.velocity.y <= 0)  //if we are not jumping
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundCollideRadius, whatIsGround);  //create colliders that check for whatIsGround (currently set to everything) 
                                                                                                                         //within groundCollideRadius (set in unity) of point.position (The PC's feet and a centre point between them)
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)   //if the colliders are colliding with a gameObject that is not the player (i.e. the ground)
                    {
                        AgentAnimator.ResetTrigger("Jump");
                        AgentAnimator.SetBool("Falling", false);  // we have finished falling (or we never were) so tell Unity we don't need to play the animation
                        return true;                            //then we are on the ground
                    }
                }
            }
        }
        return false;                   //otherwise if colliders are only colliding with the player character, we must not be on the ground
    }

    private void HandleLayers()  //If the Player character is in the air we need to have the air layer fully weighted so that we play air animations, otherwise set the weight to 0 to play ground animations.
    {
        if (!onGround)
        {
            AgentAnimator.SetLayerWeight(1, 1);  //set weight for air layer (layer 1) to full (1)
        }
        else
        {
            AgentAnimator.SetLayerWeight(1, 0); //set weight for air layer (layer 1) to none (0)
        }
    }

    public void ThrowKnife()
    {
        if (this.isFacingRight())
        {
            GameObject knife = (GameObject)Instantiate(knifePrefab, KnifePosition.position, Quaternion.Euler(new Vector3(0,0,-90)));
            knife.GetComponent<BossKnife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject knife = (GameObject)Instantiate(knifePrefab, KnifePosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            knife.GetComponent<BossKnife>().Initialize(Vector2.left);
        }
    }
}

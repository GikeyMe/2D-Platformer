using UnityEngine;
using System.Collections;

public class BossThree : Agent
{

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
    private BoxCollider2D PowerMeleeHitBox;
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

    private bool MeleePowerUp;
    private SpriteRenderer mySpriteRenderer;
    private Color originalColor;
    private float PowerUpTime;
    private float FlickerTime;
    private GameObject powerup;
    private SpriteRenderer powerupSpriteRenderer;
    private float originalRunSpeed;

    protected override void TakeDamage(int hp)
    {
        if (!Alive())
        {
            AgentAnimator.SetTrigger("Death");
            return;
        }
        base.TakeDamage(hp);
        Immunity = true;
    }

    private void LoadCompletionScreen()
    {
        Application.LoadLevel("CompletionScreen");
    }
    public void capturePowerUpInformation()
    {
        Debug.Log("Capturing PowerUp Info");
        PlayerPrefsX.SetBool("BossThreeMeleePower", MeleePowerUp);
        PlayerPrefsX.SetLong("BossThreePowerTimeRemaining", (long)PowerUpTime);
    }

    private void loadPowerUpInfo()
    {
        Debug.Log("Loading PowerUpInfo");
        MeleePowerUp = PlayerPrefsX.GetBool("BossThreeMeleePower");
        if (MeleePowerUp)
            mySpriteRenderer.color = Color.blue;
        PowerUpTime = PlayerPrefsX.GetLong("BossThreePowerTimeRemaining");
        PlayerPrefsX.SetBool("BossThreeLoad", false);
        powerup.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = mySpriteRenderer.color;
        powerup = GameObject.Find("BlueDiamond");
        powerupSpriteRenderer = powerup.GetComponent<SpriteRenderer>();
        originalRunSpeed = runSpeed;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>(), true);
        if (PlayerPrefsX.GetBool("BossThreeLoad", true))
            loadPowerUpInfo();
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

    void Update()
    {
        //prioritise getting powerup but ignore if player is closer to it
        //otherwise melee attack player.
        UpdateImmunity();
        myHealthBar.UpdateHealth(hitpoints, 800f);
            UpdatePowerUp();
            if(!MeleePowerUp && (!powerupSpriteRenderer.enabled || playerObstructingPowerup()) && !PlayerMeleeRange())
            {
                if (!(activeState is RangedAttackState))
                    SetState(new RangedAttackState());
            }
             else if (powerupSpriteRenderer.enabled && !playerObstructingPowerup() && PowerUpReachable())
            {
                if (!(activeState is SeekPowerState))
                    SetState(new SeekPowerState());
            }
            else if (!(activeState is MeleeAttackState))
                SetState(new MeleeAttackState());
        activeState.Act();
    }

    private bool PowerUpReachable() //use this to ensure boss doesn't attempt to collect powerup when it is above spike traps
    { 
        if ((powerup.transform.position.x > 20 && powerup.transform.position.x < 26) || (powerup.transform.position.x > 57.5 && powerup.transform.position.x < 63.77)
            || (powerup.transform.position.x > 95.5 && powerup.transform.position.x < 101))
            return false;
        return true;
    }

    private void UpdatePowerUp()
    {
        if (MeleePowerUp)
        {
            PowerUpTime += Time.deltaTime;
            if (PowerUpTime > 5)
                PowerUpFlicker();
            if (PowerUpTime > 8)
            {
                MeleePowerUp = false;
                mySpriteRenderer.color = originalColor;
                PowerUpTime = 0;
                runSpeed = originalRunSpeed;
            }
        }
    }

    private void PowerUpFlicker()
    {
        if (FlickerTime < 0.1)
        {
            FlickerTime += Time.deltaTime;
        }
        else if (FlickerTime >= 0.1)
        {
            if (mySpriteRenderer.color == Color.blue)
                mySpriteRenderer.color = originalColor;
            else
                mySpriteRenderer.color = Color.blue;
            FlickerTime = 0;
        }
    }

    public override void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        base.OnTriggerEnter2D(EnterredObject);
        if (Alive())
        {
            if (EnterredObject.tag == "JumpNotify")
                Jump();
            if (EnterredObject.tag == "BlueDiamond")
            {
                MeleePowerUp = true;
                mySpriteRenderer.color = Color.blue;
                runSpeed = 8;
            }
        }
        if (EnterredObject.tag == "BossDespawn" && hitpoints <= 0)
            Destroy(gameObject);
    }

    private void Jump()
    {
        Debug.Log("Called jump procedure.");
        if ((onGround))  //if the player presses space and we aren't already jumping or falling
        {            
            onGround = false;
            AgentRigidbody.AddForce(new Vector2(0, jumpSpeed));    // make the player character jump (i.e apply a positive force on the y axis)
            AgentAnimator.SetTrigger("Jump");                         // Trigger the jump animation
        }
    }

    protected override void EnableHitBox()
    {
        if (MeleePowerUp)
        {
            PowerMeleeHitBox.enabled = true;
        }
        else
            MeleeHitBox.enabled = true;

        AgentRigidbody.velocity = new Vector2((float)0.000001, AgentRigidbody.velocity.y);  //Need a tiny velocity to trigger the OnTriggerEnter event in Player.
        AgentRigidbody.velocity = new Vector2(0, AgentRigidbody.velocity.y);
    }

    protected override void DisableHitBox()
    {
        MeleeHitBox.enabled = false;
        PowerMeleeHitBox.enabled = false;
    }

    private bool playerObstructingPowerup()
    {
        if ((powerup.transform.position.x < transform.position.x) && (player.transform.position.x < transform.position.x && player.transform.position.x > powerup.transform.position.x))
            return true;
        if ((powerup.transform.position.x > transform.position.x) && (player.transform.position.x > transform.position.x && player.transform.position.x < powerup.transform.position.x))
            return true;
        return false;
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
            GameObject knife = (GameObject)Instantiate(knifePrefab, KnifePosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            knife.GetComponent<BossKnife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject knife = (GameObject)Instantiate(knifePrefab, KnifePosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            knife.GetComponent<BossKnife>().Initialize(Vector2.left);
        }
    }
}



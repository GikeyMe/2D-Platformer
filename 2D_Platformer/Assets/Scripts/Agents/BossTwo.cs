using UnityEngine;
using System.Collections;

public class BossTwo : Agent
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
    }

    void Update()
    {
        //prioritise getting powerup but ignore if player is closer to it
        //otherwise melee attack player.

        if (Alive())
        {
            UpdatePowerUp();
            if (powerupSpriteRenderer.enabled && !playerObstructingPowerup())
            {
                if (!(activeState is SeekPowerState))
                    SetState(new SeekPowerState());
            }
            else if (!(activeState is MeleeAttackState))
                SetState(new MeleeAttackState());
        }
        else
        {
            SetState(new PhaseChangeState());
            exitDoor.OpenDoor();
        }
        activeState.Act();
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
}

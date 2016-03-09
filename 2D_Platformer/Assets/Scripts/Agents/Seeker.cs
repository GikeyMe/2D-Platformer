using UnityEngine;
using System.Collections;

public class Seeker : Agent {

    private bool PlayerNearby;
    [SerializeField]
    private float HideTime;
    private float TimeHidden;
    private bool hiding;
    private bool MeleePowerUp;
    private float PowerUpTime;
    private float FlickerTime;
    private SpriteRenderer SeekerSpriteRenderer;
    private Color originalColor;
    [SerializeField]
    public BoxCollider2D PowerMeleeHitBox;

    protected override void Start()
    {
        base.Start();
        SeekerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        originalColor = SeekerSpriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (Alive())
        {
            PlayerNearby = LookForPlayer();
            UpdateHide();
            UpdatePowerUp();

            if (!hiding)
            {
                if (!Patrol && !PlayerNearby && !(activeState is IdleState))
                    SetState(new IdleState());
                else if (Patrol && !PlayerNearby && !(activeState is PatrolState))// &&!(activeState is SeekPowerState))
                    SetState(new PatrolState());
            }

            if (PlayerNearby && !IsPlayerReachable() && !(activeState is HideState))
            {
                SetState(new HideState());
                hiding = true;
            }
            else if(IsPlayerReachable() && !MeleePowerUp)
            {
                if (!(activeState is SeekPowerState))
                    SetState(new SeekPowerState());
                hiding = false;
            }
            else if (IsPlayerReachable())
            {
                if (!(activeState is MeleeAttackState))
                    SetState(new MeleeAttackState());
                hiding = false;
            }
            activeState.Act();
        }
    }

    protected override bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 20)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 5)
                return true;
        }
        return false;
    }

    private void UpdateHide()
    {
        if (hiding)
        {
            TimeHidden += Time.deltaTime;
            if (TimeHidden > HideTime)
            {
                hiding = false;
                TimeHidden = 0;
            }
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
                SeekerSpriteRenderer.color = Color.blue;
            }
        }
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
                SeekerSpriteRenderer.color = originalColor;
                PowerUpTime = 0;
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
        if (SeekerSpriteRenderer.color == Color.blue)
            SeekerSpriteRenderer.color = originalColor;
        else
            SeekerSpriteRenderer.color = Color.blue;
        FlickerTime = 0;
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
}

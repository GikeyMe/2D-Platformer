using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]  //Serialized Fields are set within unity. Makes it easier for maintenance.
    private float runSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private bool airMovement;                                        
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundCollideRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool facingRight;
    private bool onGround;

    private Animator PlayerAnimator;

    private Rigidbody2D PlayerRigidbody;

    private SpriteRenderer PlayerSpriteRenderer;

    private ArrayList MovingPlatforms = new ArrayList();
    private GameObject platform;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform BulletPosition;

    private bool OnMovingPlat;

    [SerializeField]
    private int hitpoints;

    [SerializeField]
    private BoxCollider2D MeleeHitbox;

    [SerializeField]
    private BoxCollider2D Usebox;

    private bool Immunity;
    private float ImmunityTime;
    private float FlickerTime;

    private bool JumpPowerUp;
    private float PowerUpTime;

    private Color originalColor;

    private bool OnLadder;



    // Use this for initialization
    void Start () {
        Immunity = false;
        int index = 1;
        facingRight = true;                              //we know the player character starts off facing right so set this to true
        PlayerRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        PlayerAnimator = GetComponent<Animator>();
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = PlayerSpriteRenderer.color;

        platform = GameObject.Find("MovingPlatform");                     //Add the first (non numbered) MovingPlatform to the arraylist if it exists.
        if (platform != null)
            MovingPlatforms.Add(platform);
            
        while (true)                                                                           //loop until we have all of the MovingPlatforms in the game added to this ArrayList.
        {
            platform = GameObject.Find("MovingPlatform (" + index.ToString() + ")");
            if (platform == null)  
            {
                break;
            }
            else
            {
                MovingPlatforms.Add(platform);
            }
            index++;
        }
        
    }
	
	void FixedUpdate () {
        onGround = CheckGrounded();                     //better to use a variable here so we don't need to keep calling CheckGrounded() throughout FixedUpdate, don't want to slow the game down.
        OnMovingPlat = IsPlayerOnMovingPlatform();
        float horizontal = Input.GetAxis("Horizontal"); //Unity already has keybinds set up for right and left movement. GetAxis returns a float representing the direction the player is inputting.
        UpdateImmunity();
        UpdatePowerUp();
        PlayerMovement(horizontal);  // handle player movement
        Flip(horizontal);            // do we need to flip the sprite?
        HandleLayers();
    }

    //called once per frame
    void Update()
    {
        HandleInput();                   //check for player input and deal with it as necessary
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !this.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Melee")) //if player presses melee key and we aren't already playing melee animation
        {
            PlayerAnimator.SetTrigger("melee");  //trigger the melee parameter in unity to start animation transition
            PlayerRigidbody.velocity = Vector2.zero; //stop movement on all axes while animation plays
        }

        if (Input.GetKeyDown(KeyCode.Space) && (onGround || JumpPowerUp))  //if the player presses space and we aren't already jumping or falling
        {
                onGround = false;
                PlayerRigidbody.AddForce(new Vector2(0, jumpSpeed));    // make the player character jump (i.e apply a positive force on the y axis)
                PlayerAnimator.SetTrigger("Jump");                         // Trigger the jump animation
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayerAnimator.SetBool("Shooting", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            PlayerAnimator.SetBool("Shooting", false);
        }
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            Usebox.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Usebox.enabled = false;
        }
    }

    private bool IsPlayerOnMovingPlatform()
    { 
         foreach (GameObject plat in MovingPlatforms)
          {
            if (Mathf.Abs(PlayerRigidbody.transform.position.x - plat.transform.position.x) < 16 && Mathf.Abs(PlayerRigidbody.transform.position.x - plat.transform.position.x) > 7)
            {
               if (Mathf.Abs(PlayerRigidbody.transform.position.y - plat.transform.position.y) < 0.25)
               {
                  return true;
              }
            }
           }
        return false;
    }

    private void PlayerMovement(float horizontal)
    {
        if(PlayerRigidbody.velocity.y < 0  && !(OnMovingPlat))  //if player character is falling and not on a moving platform
        {
            PlayerAnimator.SetBool("Falling", true); //play falling animation. This can't be a trigger because we are unsure how far the player character is falling and so the animation has to be played 
        }                                            // until explicitly told to stop. (i.e boolean is false again)

        if (!this.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Melee") && (onGround || airMovement)) //if we aren't already playing melee animation and we aren't jumping or falling,
        {                                                                                                   // then we are allowed to move horizontally
            PlayerRigidbody.velocity = new Vector2(horizontal * runSpeed, PlayerRigidbody.velocity.y);
            //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
            PlayerAnimator.SetFloat("speed", Mathf.Abs(horizontal));  // we don't want a negative value so use Mathf.Abs
        }  

        if(OnLadder)
        {
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * runSpeed);
            PlayerAnimator.SetFloat("ClimbSpeed", Mathf.Abs(Input.GetAxisRaw("Vertical")) * runSpeed);
        }
    }


    private void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            
            //negate the x axis scale for the player (i.e. flip the sprite)
            Vector3 theScale = transform.localScale; 
            theScale.x *= -1;
            transform.localScale = theScale;

            // due to the sprite being very wide, we also have to make a correction for the position on the x axis after the flip
            Vector3 currentPos = transform.localPosition;
            if (facingRight)
            {
                currentPos.Set(currentPos.x - (float)2.5, currentPos.y, currentPos.z);
            }
            else
            {
                currentPos.Set(currentPos.x + (float)2.5, currentPos.y, currentPos.z);
            }         
            transform.localPosition = currentPos;

            //need to change this each time we flip the player otherwise we will lose track of the direction he is facing
            facingRight = !facingRight; 
        }
    }

    private bool CheckGrounded()
    {
        if (OnMovingPlat)
            return true;
        if (PlayerRigidbody.velocity.y <= 0)  //if we are not jumping
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundCollideRadius, whatIsGround);  //create colliders that check for whatIsGround (currently set to everything) 
                                                                                                                         //within groundCollideRadius (set in unity) of point.position (The PC's feet and a centre point between them)
                for (int i=0; i < colliders.Length; i++) 
                {
                    if (colliders[i].gameObject != gameObject)   //if the colliders are colliding with a gameObject that is not the player (i.e. the ground)
                    {
                        PlayerAnimator.ResetTrigger("Jump");    
                        PlayerAnimator.SetBool("Falling", false);  // we have finished falling (or we never were) so tell Unity we don't need to play the animation
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
            PlayerAnimator.SetLayerWeight(1, 1);  //set weight for air layer (layer 1) to full (1)
        }
        else
        {
            PlayerAnimator.SetLayerWeight(1, 0); //set weight for air layer (layer 1) to none (0)
        }
    }

    public void FireBullet()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, BulletPosition.position, Quaternion.identity);
        if (facingRight)
        {  
            bullet.GetComponent<Bullet>().Initialize(Vector2.right);
        }
        else
        {
            bullet.GetComponent<Bullet>().Initialize(Vector2.left);
        }
    }

    private void UpdateImmunity()
    {
        if (Immunity)
        {
            SpriteFlicker();
            ImmunityTime += Time.deltaTime;
            if (ImmunityTime > 1)
            {
                Immunity = false;
                PlayerSpriteRenderer.enabled = true;
                ImmunityTime = 0;
            }
        }
    }

    private void UpdatePowerUp()
    {
        if (JumpPowerUp)
        {
            PowerUpTime += Time.deltaTime;
            if (PowerUpTime > 12)
                PowerUpFlicker();
            if (PowerUpTime > 15)
            {
                JumpPowerUp = false;
                PlayerSpriteRenderer.color = originalColor;
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
            if (PlayerSpriteRenderer.color == Color.red)
                PlayerSpriteRenderer.color = originalColor;
            else
                PlayerSpriteRenderer.color = Color.red;
            FlickerTime = 0;
        }
    }

    private void SpriteFlicker()
    {
        if(FlickerTime < 0.1)
        {
            FlickerTime += Time.deltaTime;
        }
        else if(FlickerTime >= 0.1)
        {
            PlayerSpriteRenderer.enabled = !PlayerSpriteRenderer.enabled;
            FlickerTime = 0;
        }
    }

    public void TakeDamage(string source, int hp)
    {
        if (source.Equals("SpikeTrigger"))              //added this code to ignore immunity when landing on spikes
        {
            hitpoints = 0;
            PlayerAnimator.SetTrigger("Death");
        }
        if (!Immunity)
        {
            hitpoints -= hp;
            Immunity = true;            
            if (Alive())
            {
                PlayerAnimator.SetTrigger("Damage");
            }
            else
            {
                PlayerAnimator.SetTrigger("Death");
            }
        }
    }

    public void StartMelee()
    {            
        MeleeHitbox.enabled = true;                                                       //Will be used by animator to switch on hitbox while player is melee attacking, kept off at all other times
        PlayerRigidbody.velocity = new Vector2((float)0.000001, PlayerRigidbody.velocity.y);  //Need a tiny velocity to trigger the OnTriggerEnter event in Agent.
    }

    public void StopMelee()
    {
        MeleeHitbox.enabled = false;
        PlayerRigidbody.velocity = new Vector2(0, PlayerRigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (Alive() && !PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Melee"))
        {
            if (EnterredObject.tag == "SlimeMelee")
            {
                TakeDamage("SlimeMelee",20);
            }

            if (EnterredObject.tag == "BatMelee")
            {
                TakeDamage("BatMelee",20);
            }

            if (EnterredObject.tag == "WormMelee")
            {
                TakeDamage("WormMelee",20);
            }

            if(EnterredObject.tag == "BossMelee")
            {
                TakeDamage("BossMelee",20);
            }

            if (EnterredObject.tag == "BossKnife")
            {
                TakeDamage("BossKnife",20);
            }

            if (EnterredObject.gameObject.name == ("SpikeTrigger"))
            {
                TakeDamage("SpikeTrigger",100);
            }

            if(EnterredObject.tag == "Diamond")
            {
                JumpPowerUp = true;
                PlayerSpriteRenderer.color = Color.red;
            }

            if(EnterredObject.tag == "Ladder")
            {
                OnLadder = true;
                PlayerRigidbody.gravityScale = 0;
                PlayerAnimator.SetBool("OnLadder", true);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D ExitObject)
    {
        if(ExitObject.tag == "Ladder")
        {
            OnLadder = false;
            PlayerRigidbody.gravityScale = 8;
            PlayerAnimator.SetBool("OnLadder", false);
        }
    }

    private bool Alive()
    {
        return hitpoints > 0;     
    }
}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]  //Serialized Fields are set within unity. Makes it easier for maintenance.
    private float runSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private bool airControl;                                        //remove this later if I don't use it for power ups  
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


    // Use this for initialization
    void Start () {
        facingRight = true;                              //we know the player character starts off facing right so set this to true
        PlayerRigidbody = GetComponent<Rigidbody2D>();   //Get the Rigidbody2D and Animator objects from unity
        PlayerAnimator = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
        onGround = CheckGrounded();                     //better to use a variable here so we don't need to keep calling CheckGrounded() throughout FixedUpdate, don't want to slow the game down.
        float horizontal = Input.GetAxis("Horizontal"); //Unity already has keybinds set up for right and left movement. GetAxis returns a float representing the direction the player is inputting.
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

        if (Input.GetKeyDown(KeyCode.Space) && onGround)  //if the player presses space and we aren't already jumping or falling
        {
                onGround = false;
                PlayerRigidbody.AddForce(new Vector2(0, jumpSpeed));    // make the player character jump (i.e apply a positive force on the y axis)
                PlayerAnimator.SetTrigger("Jump");                         // Trigger the jump animation
        }
    }

    private void PlayerMovement(float horizontal)
    {
        if(PlayerRigidbody.velocity.y < 0)  //if player character is falling
        {
            PlayerAnimator.SetBool("Falling", true); //play falling animation. This can't be a trigger because we are unsure how far the player character is falling and so the animation has to be played 
        }                                            // until explicitly told to stop. (i.e boolean is false again)

        if (!this.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Melee") && (onGround || airControl)) //if we aren't already playing melee animation and we aren't jumping or falling,
        {                                                                                                   // then we are allowed to move horizontally
            PlayerRigidbody.velocity = new Vector2(horizontal * runSpeed, PlayerRigidbody.velocity.y);
        }


        //speed is a parameter needed for unity to know when to transition from idle to run animation and vice versa
        PlayerAnimator.SetFloat("speed", Mathf.Abs(horizontal));  // we don't want a negative value so use Mathf.Abs
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
}

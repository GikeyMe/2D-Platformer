using UnityEngine;
using System.Collections;

public class Boss : Agent {

    [SerializeField]
    private GameObject knifePrefab;

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

    private bool onGround;

    // Use this for initialization
    protected override void Start () {
      //  onGround = CheckGrounded();
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update On ground = " + onGround);
        if ((onGround))  //if the player presses space and we aren't already jumping or falling
        {
            Debug.Log("Inside jump thing.");
            onGround = false;
            AgentRigidbody.AddForce(new Vector2(0, jumpSpeed));    // make the player character jump (i.e apply a positive force on the y axis)
            AgentAnimator.SetTrigger("Jump");                         // Trigger the jump animation
            Debug.Log("Applied Forces.");
        }
    }

    void FixedUpdate()
    {       
        onGround = CheckGrounded();
        Debug.Log("FixedUpdate On ground = " + onGround);
        if (AgentRigidbody.velocity.y < 0)  //if player character is falling and not on a moving platform
        {
            Debug.Log("Set Falling to True");
            AgentAnimator.SetBool("Falling", true); //play falling animation. This can't be a trigger because we are unsure how far the player character is falling and so the animation has to be played 
        }                                            // until explicitly told to stop. (i.e boolean is false again)
        Debug.Log("Call HandleLayers");
        HandleLayers();
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
                        Debug.Log("We have finished falling so reset trigger and boolean");
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

using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {

    private BoxCollider2D playerHitBox;
    [SerializeField]
    private BoxCollider2D platformCollider;
    [SerializeField]
    private BoxCollider2D platformTrigger;

	// Use this for initialization
	void Start () {
        playerHitBox = GameObject.Find("Player").GetComponent<BoxCollider2D>();  // get the player character's hitbox
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);      //ignore collision between the two collision boxes on the platform
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerHitBox, true);   //player walked through trigger box so disable collision 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerHitBox, false);  //player left trigger box so enable collision again
        }
    }
}

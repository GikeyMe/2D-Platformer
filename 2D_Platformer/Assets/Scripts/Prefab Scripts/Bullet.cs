using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D BulletRigidbody;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        BulletRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        BulletRigidbody.velocity = direction * speed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BossKnife" || collision.gameObject.tag == "Checkpoint" || collision.gameObject.tag == "Dialogue" || collision.gameObject.tag == "JumpNotify"
            || collision.gameObject.tag == "PhaseTwo" || collision.gameObject.tag == "PhaseThree" || collision.gameObject.tag == "PhaseFour" || collision.gameObject.tag == "NoSpawn")
            return;
        else
            Destroy(gameObject); //if bullets collide with anything else destroy them
    }
}

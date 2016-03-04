using UnityEngine;
using System.Collections;

public class BossKnife : MonoBehaviour
{

    [SerializeField]
    private float speed;

    private Rigidbody2D KnifeRigidbody;

    private Vector2 direction;

    // Use this for initialization
    void Start()
    {
        KnifeRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        KnifeRigidbody.velocity = direction * speed;
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
        if ((collision.tag == "PhaseTwo") || (collision.tag == "PhaseThree") || (collision.tag == "PhaseFour") || (collision.tag == "JumpNotify") || (collision.gameObject.tag == "Bullet"))
            return;
        else
            Destroy(gameObject); //if bullets collide with anything else destroy them
    }
}

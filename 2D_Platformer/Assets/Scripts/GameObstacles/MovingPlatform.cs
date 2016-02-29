using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    private Rigidbody2D PlatformRigidbody;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float StartPoint;
    [SerializeField]
    private float StopPoint;
    [SerializeField]
    private bool MoveAlongX;
    [SerializeField]
    private bool MoveAlongY;

    // Use this for initialization
    void Start () {
        PlatformRigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        movePlatform();
	}

    private void movePlatform()
    {
        if (MoveAlongY)
        {
            if (PlatformRigidbody.transform.position.y > StartPoint)
            {
                PlatformRigidbody.velocity = new Vector2(PlatformRigidbody.velocity.x, -1 * moveSpeed);
            }
            if (PlatformRigidbody.transform.position.y < StopPoint)
            {
                PlatformRigidbody.velocity = new Vector2(PlatformRigidbody.velocity.x, 1 * moveSpeed);
            }
        }
        if (MoveAlongX)
        {
            if (PlatformRigidbody.transform.position.x > StartPoint)
            {
                PlatformRigidbody.velocity = new Vector2(-1 * moveSpeed, PlatformRigidbody.velocity.y);
            }
            if (PlatformRigidbody.transform.position.x < StopPoint)
            {
                PlatformRigidbody.velocity = new Vector2(1 * moveSpeed, PlatformRigidbody.velocity.y);
            }
        }
    }
}

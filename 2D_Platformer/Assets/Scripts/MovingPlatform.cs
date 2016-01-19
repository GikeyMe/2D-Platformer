using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    private Rigidbody2D PlatformRigidbody;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float LowestPoint;
    [SerializeField]
    private float HighestPoint;

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
        if (PlatformRigidbody.transform.position.y < LowestPoint)
            PlatformRigidbody.velocity = new Vector2(PlatformRigidbody.velocity.x, 1 * moveSpeed);
        if (PlatformRigidbody.transform.position.y > HighestPoint)
            PlatformRigidbody.velocity = new Vector2(PlatformRigidbody.velocity.x, -1 * moveSpeed);
    }
}

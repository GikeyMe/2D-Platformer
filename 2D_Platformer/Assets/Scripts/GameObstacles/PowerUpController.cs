using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

    private float RespawnTime;

    [SerializeField]
    private BoxCollider2D myCollider;
    [SerializeField]
    private BoxCollider2D playerCollider;
    [SerializeField]
    private BoxCollider2D bossCollider;
    private SpriteRenderer mySpriteRenderer;
    private GameObject mainCam;
    private float NewX;



    public void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = GameObject.Find("Main Camera");
    }

    void Update()
    {
        CheckRespawnTime();
    }

    void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if(EnterredObject.name == "Player" || EnterredObject.name == "Boss")
        {
            Physics2D.IgnoreCollision(myCollider, playerCollider, true);   //player walked through trigger box so disable collision 
            Physics2D.IgnoreCollision(myCollider, bossCollider, true);
            mySpriteRenderer.enabled = false;
        }
    }

    private void CheckRespawnTime()
    {
        if (!mySpriteRenderer.enabled)
        {
            RespawnTime += Time.deltaTime;
            if (RespawnTime > 10)
            {
                setNewLocation();
                Physics2D.IgnoreCollision(myCollider, playerCollider, false);   //player walked through trigger box so disable collision 
                Physics2D.IgnoreCollision(myCollider, bossCollider, false);
                mySpriteRenderer.enabled = true;
                RespawnTime = 0;              
            }
        }
    }

    public void setNewLocation()
    { 
        float tempx = mainCam.transform.position.x;
        tempx -= 21;
        tempx = Random.Range(tempx, tempx + 42);
        NewX = tempx;
        transform.position = new Vector3(NewX, transform.position.y, transform.position.z);
    }
}

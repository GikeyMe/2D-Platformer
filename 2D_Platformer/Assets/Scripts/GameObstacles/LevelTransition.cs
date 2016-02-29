using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour
{

    private bool playerInZone;
    [SerializeField]
    private string LevelToLoad;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && playerInZone)
        {
            Application.LoadLevel(LevelToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (EnterredObject.name == "Player")
        {
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D EnterredObject)
    {
        if (EnterredObject.name == "Player")
        {
            playerInZone = false;
        }
    }
}

using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

    private Animator LeverAnimator;

    void Start()
    {
        LeverAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (EnterredObject.tag == "PlayerUseBox")
        {
            LeverAnimator.SetBool("Pulled", true);
        }
    }
}

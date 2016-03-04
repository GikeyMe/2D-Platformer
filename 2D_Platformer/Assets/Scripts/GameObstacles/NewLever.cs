using UnityEngine;
using System.Collections;

public class NewLever : MonoBehaviour {

    private Animator LeverAnimator;

    // Use this for initialization
    void Start () {
        LeverAnimator = GetComponent<Animator>();

        if (PlayerPrefsX.GetBool("LeverLoading"))
        {
            if (gameObject.name == "LeftLever")
            {
                LeverAnimator.SetBool("Pulled", PlayerPrefsX.GetBool("LeftLeverPulled"));
                PlayerPrefsX.SetBool("LeftLeverDealtWith", true);
            }
            if (gameObject.name == "RightLever")
            {
                LeverAnimator.SetBool("Pulled", PlayerPrefsX.GetBool("RightLeverPulled"));
                PlayerPrefsX.SetBool("RightLeverDealtWith", true);
            }

            if (PlayerPrefsX.GetBool("LeftLeverDealtWith") && PlayerPrefsX.GetBool("RightLeverDealtWith"))
            {
                PlayerPrefsX.SetBool("LeverLoading", false); //this line is what is causing the issues with the left lever
                PlayerPrefsX.SetBool("LeftLeverDealtWith", false);
                PlayerPrefsX.SetBool("RightLeverDealtWith", false);
            }
        }


    }

    void OnTriggerEnter2D(Collider2D EnterredObject)
    {
        if (EnterredObject.tag == "PlayerUseBox")
        {
            LeverAnimator.SetBool("Pulled", true);
        }
    } 
}

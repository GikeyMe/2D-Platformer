using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour {
    [SerializeField]
    private GameObject LeftLever;
    private Animator LLAnimator;
    [SerializeField]
    private GameObject RightLever;
    private Animator RLAnimator;
    [SerializeField]
    private GameObject LeftSwitch;
    private Animator LSAnimator;
    [SerializeField]
    private GameObject RightSwitch;
    private Animator RSAnimator;    
    private Animator ExitDoorAnimator;
    [SerializeField]
    private Collider2D ExitTrigger;

    // Use this for initialization
    void Start () {
        ExitDoorAnimator = GetComponent<Animator>();
        LLAnimator = LeftLever.GetComponent<Animator>();
        RLAnimator = RightLever.GetComponent<Animator>();
        LSAnimator = LeftSwitch.GetComponent<Animator>();
        RSAnimator = RightSwitch.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (LLAnimator.GetBool("Pulled"))
            LSAnimator.SetBool("SwitchedOn",true);
        if (RLAnimator.GetBool("Pulled"))
            RSAnimator.SetBool("SwitchedOn", true);
        if (LSAnimator.GetBool("SwitchedOn") && RSAnimator.GetBool("SwitchedOn"))
        {
            ExitDoorAnimator.SetBool("SwitchesAreOn", true);
            ExitTrigger.enabled = true;
        }
    }
}

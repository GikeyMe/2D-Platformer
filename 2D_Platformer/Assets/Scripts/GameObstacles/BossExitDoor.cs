using UnityEngine;
using System.Collections;

public class BossExitDoor : MonoBehaviour {

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
        LSAnimator = LeftSwitch.GetComponent<Animator>();
        RSAnimator = RightSwitch.GetComponent<Animator>();
    }
	
	public void OpenDoor()
    {
        RSAnimator.SetBool("SwitchedOn", true);
        LSAnimator.SetBool("SwitchedOn", true);
        ExitDoorAnimator.SetBool("SwitchesAreOn", true);
        ExitTrigger.enabled = true;
    }
}

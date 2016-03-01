using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private Image HealthImage;
	
	public void UpdateHealth (int currenthp, float MaxHp) {
        HealthImage.fillAmount = (currenthp / MaxHp);
	}



}

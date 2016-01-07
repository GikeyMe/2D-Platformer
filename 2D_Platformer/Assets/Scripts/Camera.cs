using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    [SerializeField]
    private float xLimit;
    [SerializeField]
    private float yLimit;

    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMin;

    private Transform PlayerPosition;



	// Use this for initialization
	void Start () {

        PlayerPosition = GameObject.Find("Player").transform;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(Mathf.Clamp(PlayerPosition.position.x, xMin, xLimit), Mathf.Clamp(PlayerPosition.position.y, yMin, yLimit), transform.position.z);
	}
}

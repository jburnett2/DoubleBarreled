using UnityEngine;
using System.Collections;

public class shootAnimate : MonoBehaviour {

    public int playerNumber;
    private Animator animationController;

	// Use this for initialization
	void Start () {
        animationController = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        animationController.SetFloat("shoot", Input.GetAxisRaw("Shoot" + playerNumber));
	}
}

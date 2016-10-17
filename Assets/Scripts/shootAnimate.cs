using UnityEngine;
using System.Collections;

public class shootAnimate : MonoBehaviour {

    public int playerNumber;

    private bool shooting;
    private Animator animationController;

	// Use this for initialization
	void Start () {
        animationController = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Shoot" + playerNumber) == 0)
        {
            shooting = false;
        }
        else
        {
            shooting = true;
        }
        animationController.SetBool("shoot", shooting);
	}
}

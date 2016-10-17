using UnityEngine;
using System.Collections;

public class reloadAnimate : MonoBehaviour {

    public int playerNumber;
    private Animator animationController;

	// Use this for initialization
	void Start () {
        animationController = GetComponent<Animator>();
        animationController.speed = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
        animationController.SetBool("reload", Input.GetButtonDown("Reload" + playerNumber));
        if (animationController.GetCurrentAnimatorStateInfo(0).IsName("reload"))
        {
            Debug.Log("i think this is working");
        }
	}
}

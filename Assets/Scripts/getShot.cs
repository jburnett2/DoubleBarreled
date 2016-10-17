using UnityEngine;
using System.Collections;

public class getShot : MonoBehaviour {

    public GameObject legs;

    private SpriteRenderer sr;
    private SpriteRenderer lsr;

    private bool dead = false;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        lsr = legs.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if (dead)
        {
            sr.flipX = true;
            lsr.flipX = true;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            dead = !dead;
        }
    }
}

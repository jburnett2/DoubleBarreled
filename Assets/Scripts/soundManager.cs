using UnityEngine;
using System.Collections;

public class soundManager : Observer {

    public AudioClip jump;
    public AudioClip doublejump;
    public AudioClip shoot;
    public AudioClip hurt;

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void onNotify(string description)
    {
        if (description == "jump")
        {
            source.PlayOneShot(jump, 0.75f);
        }
        else if (description == "doublejump")
        {
            source.PlayOneShot(doublejump, 0.75f);
        }
        else if (description == "shoot")
        {
            source.PlayOneShot(shoot, 0.75f);
        }
        else if (description == "hurt")
        {
            source.PlayOneShot(hurt, 1f);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Observer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // happens before start
    void Awake()
    {

    }

    public void onNotify(Observable sender, string description)
    {

    }
}

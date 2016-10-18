using UnityEngine;
using System.Collections;

abstract public class Observer : MonoBehaviour {

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

    abstract public void onNotify(string description);

}

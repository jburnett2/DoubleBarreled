using UnityEngine;
using System.Collections;

public class levelLoop : MonoBehaviour {

    public Transform target;
    Camera mainCam;

	// Use this for initialization
	void Start () {
        mainCam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);
        if (screenPos.x > mainCam.pixelWidth)
        {
            //target.position = new Vector2(2f, target.position.y);
            screenPos = new Vector3(0, screenPos.y, screenPos.z);
        }
	}
}

using UnityEngine;
using System.Collections;

public class pauseMenu : MonoBehaviour {

    public bool isPaused;
    public GameObject pmCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
        {
            pmCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pmCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
        }
    }

    public void resume()
    {
        isPaused = false;
    }

    public void quit()
    {
        Application.Quit();
    }
}

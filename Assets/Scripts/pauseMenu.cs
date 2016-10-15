using UnityEngine;
using System.Collections;

public class pauseMenu : MonoBehaviour {

    private bool isPaused;
    public GameObject pmCanvas;

    void Start()
    {
       //pmCanvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (isPaused)
        {
            pmCanvas.SetActive(true);
            Time.timeScale = 0;
        } 
        else
        {
            pmCanvas.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
        }
    }

    public void Resume()
    {
        Debug.Log("Resume");
        isPaused = false;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour {

    public float startNewLevelDelayTime;
    private float startNewLevelTime = 0;
    
    void Start ()
    {
        startNewLevelTime = Time.time;
    }
	
	void FixedUpdate()
    {
        if (Time.fixedTime > startNewLevelTime + startNewLevelDelayTime)
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }

        if (Input.GetKeyDown(KeyCode.F1) || Input.GetButton("Fire1") || Input.GetButton("Submit"))
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

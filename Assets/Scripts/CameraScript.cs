using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    int i = 0;

    public float sensitivity;
    public int screenshot = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Jump"))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F12) || Input.GetButtonDown("Fire3"))
        {
            UnityEngine.XR.InputTracking.Recenter();
        }

        if (Input.GetKeyDown(KeyCode.F1))
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

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ScreenCapture.CaptureScreenshot("Screenshot " + SceneManager.GetActiveScene().buildIndex + " " + i++ + ".png");
            print("screenshot");
        }


        if (Input.GetKeyDown(KeyCode.F4))
        {
            ScreenCapture.CaptureScreenshot("Screenshot"+screenshot+ Application.loadedLevelName+".png", 2);
            screenshot++;
        }

#if !UNITY_ANDROID && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
#endif

    }
}

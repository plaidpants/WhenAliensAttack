using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseGame : MonoBehaviour
{
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        PlayerTracker.SetTrackingObject(gameObject);
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1") || Input.GetButton("Submit") || (audio.isPlaying == false))
        {
            SceneManager.LoadScene(0);
        }
    }
}

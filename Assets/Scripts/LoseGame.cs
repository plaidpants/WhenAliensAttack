using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseGame : MonoBehaviour
{
    AudioSource loseAudio;

    void Start()
    {
        loseAudio = GetComponent<AudioSource>();
        PlayerTracker.SetTrackingObject(gameObject);
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit") || (loseAudio.isPlaying == false))
        {
            SceneManager.LoadScene(0);
        }
    }
}

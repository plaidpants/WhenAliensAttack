using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	
    void Start()
    {
        PlayerTracker.SetTrackingObject(gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(2);
        }
    }
}

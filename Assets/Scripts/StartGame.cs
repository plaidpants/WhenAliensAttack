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
        if (Input.GetButton("Fire1") || Input.GetButton("Submit"))
        {
            SceneManager.LoadScene(2);
        }
    }
}

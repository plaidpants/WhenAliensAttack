using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Alien")
        {
            SceneManager.LoadScene(1);
        }
    }
}

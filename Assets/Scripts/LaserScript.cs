using UnityEngine;
using System.Collections;


public class LaserScript : MonoBehaviour {

    public static int count = 0;
    public GameObject explosionPrefab;
    public AudioClip deathSound;

    void OnTriggerEnter(Collider collider)
    {
        if (gameObject.name.Contains("LaserPlayer") && collider.gameObject.name.Contains("LaserAlien"))
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(collider.gameObject);
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        }
    }

    void OnDestroy()
    {
        // keep track of how many there are of these
        count--;
    }

	void Start ()
    {
        // keep track of how many there are of these
        count++;
    }
}

using UnityEngine;
using System.Collections;

public class BarrierScript : MonoBehaviour {

    public AudioClip destructionSound;
    public GameObject explosionPrefab;
    
    void OnTriggerEnter (Collider collider)
	{
		AudioSource.PlayClipAtPoint(destructionSound, transform.position);
        if (collider.gameObject.tag != "Alien")
        {
            Destroy(collider.gameObject);
        }
		Destroy(gameObject);
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
    }
}

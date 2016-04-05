using UnityEngine;
using System.Collections;

public class AlienLaser : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip deathSound;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("Moon Terrain"))
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        }
    }
}

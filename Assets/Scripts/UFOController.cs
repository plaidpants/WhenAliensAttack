using UnityEngine;
using System.Collections;

public class UFOController : MonoBehaviour {
    
    public GameObject explosionPrefab;
    public AudioClip deathSound;
    public float speed;
    public float rotationSpeed;
    public float min;
    public float max;
    public static int count = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Untagged")
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(collider.gameObject);
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        }

        if ((transform.position.x > max) || (transform.position.x < min))
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        count++;
        GetComponent<Rigidbody>().velocity = Vector3.right * speed;
        GetComponent<Rigidbody>().angularVelocity = Vector3.down * rotationSpeed ;
    }

    void FixedUpdate ()
    {
        if ((transform.position.x > max) || (transform.position.x < min))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        count--;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class UFOControllerCircle : MonoBehaviour {
    public GameObject explosionPrefab;
    public AudioClip deathSound;
    public float speed;
    public float rotationSpeed;
    private float positionAngle;
    public static int count = 0;
    public float radius;
    public Vector3 center;
    public float MaxLoopDegrees;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Untagged")
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(collider.gameObject);
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        }
    }

    // Use this for initialization
    void Start()
    {
        count++;

        // start it spinning
        GetComponent<Rigidbody>().angularVelocity = Vector3.down * rotationSpeed;

        // start position 
        positionAngle = 0;

        // the X & Y position for this angle are calculated using Sin & Cos
        float x = Mathf.Sin(positionAngle) * radius;
        float y = Mathf.Cos(positionAngle) * radius;
        Vector3 pos = new Vector3(x, 0, y) + center;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

        GetComponent<Rigidbody>().MovePosition(pos);
    }

    void FixedUpdate()
    {
        positionAngle = positionAngle + speed * Time.fixedDeltaTime;

        // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
        float x = Mathf.Sin(positionAngle) * radius;
        float y = Mathf.Cos(positionAngle) * radius;
        Vector3 pos = new Vector3(x, 0, y) + center;

        GetComponent<Rigidbody>().MovePosition(pos);

        if (positionAngle > Mathf.Deg2Rad * MaxLoopDegrees)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        count--;
    }
}

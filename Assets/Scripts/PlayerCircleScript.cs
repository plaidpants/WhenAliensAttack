using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class PlayerCircleScript : MonoBehaviour {
    public float speed;
    public GameObject playerLaserPrefab;
    public float projectileSpeed;
    public float projectileRepeatRate;
    public AudioClip projectileSound;
    public AudioClip deathSound;
    public GameObject explosionPrefab;
    public int playerMaxShots;
    public Vector3 center;
    public float radius;

    private float positionAngle = 0;

    private bool button = false;
    static public int count = 0;

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

    void OnDestroy()
    {
        count--;
    }

    // Use this for initialization
    void Start()
    {
        count++;

        PlayerTracker.SetTrackingObject(gameObject);

        // start position where we are currently looking
        Quaternion quaternion = InputTracking.GetLocalRotation(VRNode.CenterEye);

        Vector3 euler = quaternion.eulerAngles;

        positionAngle = euler.y * Mathf.Deg2Rad + 0.01f;

        // the X & Y position for this angle are calculated using Sin & Cos
        float x = Mathf.Sin(positionAngle) * radius;
        float y = Mathf.Cos(positionAngle) * radius;
        Vector3 pos = new Vector3(x, 0, y) + center;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos) * Quaternion.Euler(-90, 0, 0);

        GetComponent<Rigidbody>().MovePosition(pos);
        GetComponent<Rigidbody>().MoveRotation(rot);
    }

    void FireProjectile()
    {
        if (LaserScript.count < playerMaxShots)
        {
            GameObject laser = Instantiate(playerLaserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody>().velocity = new Vector3(0f, projectileSpeed, 0);
            AudioSource.PlayClipAtPoint(projectileSound, laser.transform.position);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            positionAngle = positionAngle - speed * Time.fixedDeltaTime;

            // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
            float x = Mathf.Sin(positionAngle) * radius;
            float y = Mathf.Cos(positionAngle) * radius;
            Vector3 pos = new Vector3(x, 0, y) + center;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos) * Quaternion.Euler(-90, 0, 0);

            GetComponent<Rigidbody>().MovePosition(pos);
            GetComponent<Rigidbody>().MoveRotation(rot);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            positionAngle = positionAngle + speed * Time.fixedDeltaTime;

            // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
            float x = Mathf.Sin(positionAngle) * radius;
            float y = Mathf.Cos(positionAngle) * radius;
            Vector3 pos = new Vector3(x, 0, y) + center;

            Quaternion rot;
            if (positionAngle == Mathf.PI)
            {
                rot = Quaternion.FromToRotation(Vector3.forward, center - pos) * Quaternion.Euler(-90, 0, 0) * Quaternion.Euler(0, 0, 180);
            }
            else
            {
                rot = Quaternion.FromToRotation(Vector3.forward, center - pos) * Quaternion.Euler(-90, 0, 180);
            }
            
            GetComponent<Rigidbody>().MovePosition(pos);
            GetComponent<Rigidbody>().MoveRotation(rot);
        }

        if (Input.GetButton("Fire1") && button == false)
        {
            button = true;
            InvokeRepeating("FireProjectile", 0.0001f, projectileRepeatRate);
        }
        else if (!Input.GetButton("Fire1") && button == true)
        {
            button = false;
            CancelInvoke("FireProjectile");
        }
    }
}

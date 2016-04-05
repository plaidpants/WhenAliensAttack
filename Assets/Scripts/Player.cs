using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed;
	public float padding;
	public GameObject playerLaserPrefab;
	public float projectileSpeed;
	public float projectileRepeatRate;
	public AudioClip projectileSound;
	public float health = 250f;
	public AudioClip deathSound;
    public GameObject explosionPrefab;
    public int playerMaxShots;
    public float min;
    public float max;
    public float deadzone;
    public float mousedeadzone;
    
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
    void Start ()
    {
        PlayerTracker.SetTrackingObject(gameObject);
        count++;
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

	void FixedUpdate () 
    {
        if (((Input.GetAxis("Horizontal") < -deadzone)
#if UNITY_ANDROID && !UNITY_EDITOR
           || (Input.mousePosition.x  < Screen.width / 2 - Screen.width* mousedeadzone)
#endif
            )
            && (transform.position.x > min))
        {
            GetComponent<Rigidbody>().MovePosition( new Vector3(
				Mathf.Clamp(transform.position.x - speed * Time.deltaTime, min, max), 
				transform.position.y, 
				transform.position.z));
		}
		else if (((Input.GetAxis("Horizontal") > deadzone)
#if UNITY_ANDROID && !UNITY_EDITOR
            || (Input.mousePosition.x > Screen.width / 2 + Screen.width*mousedeadzone)
#endif
            )
            && (transform.position.x < max))
		{
            GetComponent<Rigidbody>().MovePosition( new Vector3(
				Mathf.Clamp(transform.position.x + speed * Time.deltaTime, min, max), 
				transform.position.y, 
				transform.position.z));
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

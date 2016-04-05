using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {
    
	public GameObject enemyLaserPrefab;
	public GameObject explosionPrefab;
	public float projectileSpeed;
	public float shotsPerSeconds;
    public AudioClip deathSound;
    FormationScript formationScript;
    public GameObject formation;
    private bool freetofire;

    void OnTriggerEnter(Collider collider)
	{
        if (collider.gameObject.tag != "Untagged")
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Destroy(collider.gameObject);

            if (collider.gameObject.tag != ("Barrier"))
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
                formationScript.alienFormationMeshUpdateTrigger1 = true;
                formationScript.alienFormationMeshUpdateTrigger2 = true;
            }
        }
 	}

    void OnDestroy()
    {
        if (formationScript)
        {
            formationScript.SpeedUp();
        }
    }
    
    void Start()
    {
        formationScript = GetComponentInParent<FormationScript>() as FormationScript;
        freetofire = false;
    }

    void FireProjectile()
    {
        if (freetofire == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit) == true)
            {
                if (hit.collider.gameObject.tag != "Alien")
                {
                    freetofire = true;
                }
            }
        }
        
        if (freetofire == true)
        {
            GameObject laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody>().velocity = new Vector3(0f, -projectileSpeed, 0);
        }
    }

    void FixedUpdate ()
    {
		float probability = shotsPerSeconds * Time.fixedDeltaTime;
		if (Random.value < probability)
		{
			FireProjectile();
		}
	}
}

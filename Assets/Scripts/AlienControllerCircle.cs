using UnityEngine;
using System.Collections;

public class AlienControllerCircle : MonoBehaviour {

    public GameObject enemyLaserPrefab;
    public GameObject explosionPrefab;
    public float projectileSpeed;
    public float shotsPerSeconds;
    public AudioClip deathSound;
    FormationCircleScript formationScript;
    public GameObject formation;

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
        formationScript = GetComponentInParent<FormationCircleScript>() as FormationCircleScript;
    }

    void FireProjectile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit) == true)
        {
            if (hit.collider.gameObject.tag != "Alien")
            {
                GameObject laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody>().velocity = new Vector3(0f, -projectileSpeed, 0);
            }
        }
    }
    
    void FixedUpdate()
    {
        float probability = shotsPerSeconds * Time.fixedDeltaTime;
        if (Random.value < probability)
        {
            FireProjectile();
        }
    }
}

using UnityEngine;
using System.Collections;

public class ExplosionScipt : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Vector3 offset = Random.onUnitSphere * 0.01f;
        GetComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(0.5f, 10);
        GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * Random.Range(0.5f, 10);
        transform.rotation = Random.rotation;
    }
}

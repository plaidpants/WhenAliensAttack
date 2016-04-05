using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	void OnTriggerEnter (Collider collider)
	{
		Destroy(collider.gameObject);
	}
}

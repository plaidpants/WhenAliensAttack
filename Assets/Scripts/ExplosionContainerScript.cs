using UnityEngine;
using System.Collections;

public class ExplosionContainerScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 5);
    }
}

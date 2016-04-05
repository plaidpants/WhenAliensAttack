using UnityEngine;
using System.Collections;

public class MoonScript : MonoBehaviour {

    public float rotationSpeed;
    
    void Start ()
    {
        // start the moon rotating
        GetComponent<Rigidbody>().angularVelocity = Vector3.left * rotationSpeed;
    }

}

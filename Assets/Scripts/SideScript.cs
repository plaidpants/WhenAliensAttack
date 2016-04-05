using UnityEngine;
using System.Collections;

public class SideScript : MonoBehaviour {
    public GameObject formation;
    public bool leftorright;
    public GameObject otherSide;

    FormationScript formationScript;

    void OnTriggerEnter(Collider collider)
    {
        if (leftorright)
        {
            formationScript.ToggleRight();
        }
        else
        {
            formationScript.ToggleLeft();
        }
    }

    // Use this for initialization
    void Start ()
    { 
        formationScript = formation.GetComponent("FormationScript") as FormationScript;
    }
}
